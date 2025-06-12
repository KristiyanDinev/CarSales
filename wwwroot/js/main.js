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

function page(pageNumber, curPage) {
    let elements = document.getElementsByClassName("car-row")
    if (elements.length < 10 && pageNumber > curPage) {
        alert("You are already on the last page.")
        return
    }

    window.location.href = window.location.pathname +
        `?${new URLSearchParams({ page: pageNumber }).toString()}`
}