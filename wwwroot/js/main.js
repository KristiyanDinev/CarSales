async function logout() {
    try {
        const response = await fetch('/logout', {
            method: 'POST'
        });

        if (response.ok) {
            window.location.pathname = '/login';
        }

    } catch {
        return
    }
}