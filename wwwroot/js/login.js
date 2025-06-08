

async function login() {
    const formData = new FormData();
    formData.append("Email", document.getElementById("Email").value);
    formData.append("Password", document.getElementById("Password").value);
    formData.append("RememberMe", document.getElementById("RememberMe").value !== null);

    try {
        const response = await fetch("/login", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            window.location.pathname = "/cars"

        } else {
            window.location.reload()
        }

    } catch (error) {
    }
}