// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//card view for long values

let movies = document.querySelectorAll('.movie');
let icons = document.querySelectorAll('.icon');
let details = document.querySelector("#movie-details");

document.addEventListener('click', (event) => {
    /*const controller = new AbortController();
    const signal = controller.signal;
    signal.addEventListener("abort", () => {
        console.log("new request. aborting previous request.");
    })*/
    let movie = event.target.closest('.movie');
    
    //icons
    if (event.target.matches('.movie-icons .icon')) {
        //console.log(window.fetch);
        document.querySelector(".loading").classList.remove('done');
        icons.forEach(x => x.classList.remove('active'));    
        event.target.classList.add('active');

        //get content
        let ids = event.target.dataset.ids;
        let altWords = event.target.getAttribute('alt').split(' ');
        let endpoint = 'GetPeople';

        switch (altWords[altWords.length-1]) {
            case 'planets':
                endpoint = 'GetPlanets';
                break;
            case 'starships':
                endpoint = 'GetStarships';
                break;
            case 'vehicles':
                endpoint = 'GetVehicles';
                break;
            default:
                break;
        }

        postData(`/Home/${endpoint}?` + new URLSearchParams({
               ids: event.target.dataset.ids
        }))
        .then(data => {
            document.querySelector(".loading").classList.add('done');
            let details = document.querySelector('#movie-details');
            details.innerHTML = '';
            details.classList = '';
            let key = Object.keys(data)[0];
            
            data[key].map(function (val) {
                let card = createCard(val);
                details.insertAdjacentHTML('beforeend', card);
            });            
        }).catch(err => {
            document.querySelector(".loading").classList.add('done');
            if (err.name === 'AbortError') {
                console.log('Fetch aborted');
            } else {
                console.error('Error occurred.', err);
            }
        });

        
    }
    if (document.querySelector('.movie.active') !== null && movie == null) {
        movies.forEach(x => x.classList = 'movie');
        document.querySelector('.intro.playIntro').classList = 'intro hide';
        details.classList = 'hide';
        icons.forEach(x => x.classList.remove('active')); 
    }
    if (document.querySelector('.movie.active') == null && movie !== null) {
        movies.forEach(x => x.classList = 'movie fade');
        let el = movie;
        movie.classList = 'movie active';
        movie.addEventListener('animationend', function playIntro() {
            el.querySelector('.intro.hide').classList = 'intro playIntro';
            el.removeEventListener('animationend', playIntro);
        });
    }
});

function createCard(obj) {
    //console.log(obj);
    let card = `
     <div class="card">
      <p class="card-title">${obj.name}</p>
      <ul>`;

    Object.keys(obj).map(function (key, index) {
        if (key != 'name') {
            let splitCamel = key.replace(/([a-z0-9])([A-Z])/g, '$1 $2');
            card += `<li><span class="card-label">${splitCamel}</span> ${obj[key]}</li>`
        }
    });

    card += '</ul></div><!--/card-->';
    return card;
}
async function postData(url = '') {
    const response = await fetch(url, {
        method: 'GET'
    });
    return response.json(); 
}