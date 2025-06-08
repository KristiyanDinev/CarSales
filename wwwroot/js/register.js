

async function register() {
    const formData = new FormData();
    formData.append("UserName", document.getElementById("UserName").value);
    formData.append("Email", document.getElementById("Email").value);
    formData.append("Password", document.getElementById("Password").value);
    formData.append("ConfirmPassword", document.getElementById("ConfirmPassword").value);

    try {
        const response = await fetch("/register", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            window.location.pathname = "/login";

        } else {
            window.location.reload()
        }

    } catch (error) {
    }
}