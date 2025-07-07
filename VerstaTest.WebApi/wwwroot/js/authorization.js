async function Input(url, message) {
    let login = document.getElementById("login").value;
    let password = document.getElementById("password").value;
    const response = await fetch(url, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            login: login,
            password: password
        })
    })
    if (response.ok == true) {
        const answer = await response.json();
        if (answer.id != -1) {
            localStorage.clear();
            localStorage.setItem('userId', answer.id);
            window.location.href = '/order/home/';
        }
        else {
            alert(message);
        }
    }
}