function saveuser()
{
    var name = document.getElementById('form_name').value ;
    var length = document.getElementById('form_length').value ;
    var weight = document.getElementById('form_weight').value ;
    var fingerprint="asdfere";
    var browser="chrome";
    
    var data = "{ fingerprint: '" + fingerprint + "', name: '" + name + "', length: '" + length + "', weight: '" + weight + "', browser: '" + browser + "'  }";

    fetch("http://localhost:7205/api/Store",
        {
            method: 'post',
            headers: {
                'Content-Type': 'application/text',
            },
            body: data
        })
        .then(response => response.json())
        .then(data =>
        {
            localStorage.setItem('currentUser', JSON.stringify(data) );
        })
        .catch((error) => {
            console.log("Failed to store user data");
        });
}