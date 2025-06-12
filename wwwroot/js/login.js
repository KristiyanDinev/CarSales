
const toggle = document.getElementById('togglePassword');
const passwordInput = document.getElementById('Password');

toggle.addEventListener('click', function () {
    const type = passwordInput.type === 'password' ? 'text' : 'password';
    passwordInput.type = type;
    this.classList.toggle('bi-eye-slash');
    this.classList.toggle('bi-eye');
});


async function login() {
    const formData = new FormData();
    formData.append("Email", document.getElementById("Email").value);
    formData.append("Password", document.getElementById("Password").value);
    formData.append("RememberMe", document.getElementById("RememberMe").checked);

    try {
        const response = await fetch("/login", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            window.location.pathname = "/cars"

        } else {
            window.location.reload();
        }

    } catch (error) {
    }
}