function toggleCarSearch() {
    document.getElementById("searchCars").style.display = "block";
    document.getElementById("searchCarsTitle").innerText = "Search Cars";
    clearSearchForm();
}

function cancelCarSearch() {
    document.getElementById("searchCars").style.display = "none";
    clearSearchForm();
}

function clearSearchForm() {
    document.getElementById('MakeSearch').value = '';
    document.getElementById('ModelSearch').value = '';
    document.getElementById('YearSearch').value = '';

    document.getElementById('PriceAsc').checked = false;
    document.getElementById('PriceDesc').checked = false;
    document.getElementById('YearAsc').checked = false;
    document.getElementById('YearDesc').checked = false;
}

function seachCars() {
    let make = document.getElementById('MakeSearch').value
    let model = document.getElementById('ModelSearch').value
    let year = document.getElementById('YearSearch').value

    let data = {
        page: 1
    }

    if (make) {
        data.make = make
    }

    if (model) {
        data.model = model
    }

    if (year) {
        data.year = Number(year)
    }

    let PriceAsc = document.getElementById('PriceAsc').checked
    let PriceDesc = document.getElementById('PriceDesc').checked
    let YearAsc = document.getElementById('YearAsc').checked
    let YearDesc = document.getElementById('YearDesc').checked

    if (PriceAsc || YearAsc) {
        data.SortDescending = false
    }

    if (PriceDesc || YearDesc) {
        data.SortDescending = true
    }

    if (PriceAsc || PriceDesc) {
        data.SortBy = 0

    }

    if (YearAsc || YearDesc) {
        data.SortBy = 1
    }

    window.location.href = window.location.pathname + `?${new URLSearchParams(data).toString()}`
}