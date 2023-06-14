import getFingerprint from './get-browser-fingerprint.js';
const fingerprint = getFingerprint({ debug: true }); 

export function saveuser()
{
    var name = document.getElementById('form_name').value ;
    var length = document.getElementById('form_length').value ;
    var weight = document.getElementById('form_weight').value ;
    var browser=navigator.userAgent;
    
    var data = "{ fingerprint: '" + fingerprint + "', name: '" + name + "', length: '" + length + "', weight: '" + weight + "', browser: '" + browser + "'  }";

    fetch("http://localhost:7205/api/Store",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/text',
            },
            body: data
        })
        .then(response => response)
        .then(data =>
        {
            localStorage.setItem('currentUser', JSON.stringify(data) );
            location.reload()
        })
        .catch((error) => {
            console.log("Failed to store user data");
        });    
}

$(document).ready(function () {
    console.log("Loading data to table...");
    $('#userstable').DataTable(
        {
            ajax: { url: 'http://localhost:7205/api/Retrieve', dataSrc: "" },
            columns: [
                { data: 'timestamp' },
                { data: 'fingerprint' },
                { data: 'name' },
                { data: 'length' },
                { data: 'weight' },
                { data: 'browser' }
            ],
        }
    );
});