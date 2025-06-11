const togglePassword = document.getElementById('togglePassword')
const passwordInput = document.getElementById('Password');

togglePassword.addEventListener('click', function () {
    const type = passwordInput.type === 'password' ? 'text' : 'password';
    passwordInput.type = type;
    this.classList.toggle('bi-eye-slash');
    this.classList.toggle('bi-eye');
});


const togglePassword1 = document.getElementById('togglePassword1')
const ConfirmPasswordInput = document.getElementById('ConfirmPassword');

togglePassword1.addEventListener('click', function () {
    const type = ConfirmPasswordInput.type === 'password' ? 'text' : 'password';
    ConfirmPasswordInput.type = type;
    this.classList.toggle('bi-eye-slash');
    this.classList.toggle('bi-eye');
});


async function register() {
    const formData = new FormData();
    formData.append("UserName", document.getElementById("UserName").value);
    formData.append("Email", document.getElementById("Email").value);
    formData.append("PhoneNumber", document.getElementById("PhoneNumber").value);
    formData.append("Password", document.getElementById("Password").value);
    formData.append("ConfirmPassword", document.getElementById("ConfirmPassword").value);

    try {
        const response = await fetch("/register", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            window.location.pathname = "/cars";

        } else {
            window.location.reload()
        }

    } catch (error) {
    }
}