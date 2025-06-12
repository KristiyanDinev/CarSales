
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById('carImagePreview').src = e.target.result
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function toggleForm() {
    document.getElementById("carFormContainer").style.display = "block";
    document.getElementById("formTitle").innerText = "Add New Car";
    document.getElementById('carSave').onclick = function () { saveCar() };
    clearForm();
    document.getElementById('carImage').onchange = (e) => {
        readURL(e.target); 
    }
}

function cancelEdit() {
    document.getElementById("carFormContainer").style.display = "none";
    clearForm();
}

function clearForm() {
    document.getElementById('carMake').value = '';
    document.getElementById('carModel').value = '';
    document.getElementById('carYear').value = '';
    document.getElementById('carPrice').value = '';
    document.getElementById('carColor').value = '';
    document.getElementById('carDescription').value = '';
    document.getElementById('carImage').value = '';
    document.getElementById('carImagePreview').src = '';
}


function editCar(carId, Make, Model, Year, Price, Description, Color, Image) {
    document.getElementById("carFormContainer").style.display = "block";
    document.getElementById("formTitle").innerText = "Edit Car";
    document.getElementById('carSave').onclick = function () { submitEditCar(carId) };
    clearForm();
    document.getElementById('carMake').value = Make;
    document.getElementById('carModel').value = Model;
    document.getElementById('carYear').value = Year;
    document.getElementById('carPrice').value = Price;
    document.getElementById('carColor').value = Color;
    document.getElementById('carDescription').value = Description;
    document.getElementById('carImage').value = '';
    document.getElementById('carImagePreview').src = Image;
}


async function submitEditCar(carId) {
    if (!confirm("Are you sure you want to edit this car?")) return;

    const make = document.getElementById('carMake').value
    const model = document.getElementById('carModel').value
    const year = document.getElementById('carYear').value
    const price = document.getElementById('carPrice').value
    const color = document.getElementById('carColor').value
    const description = document.getElementById('carDescription').value
    const ImageFile = document.getElementById('carImage').files[0]

    if (!make || !model || !year || !price || !color ||
        !description || !carId) {
        alert("Please fill all the inputs.");
        return;
    }

    let formData = new FormData();
    formData.append("Image", ImageFile);
    formData.append("Make", make);
    formData.append("Model", model);
    formData.append("Year", year);
    formData.append("Price", price);
    formData.append("Color", color);
    formData.append("Description", description);
    formData.append("Id", carId);

    try {
        await fetch('/admin/car/edit', {
            method: 'POST',
            body: formData
        })

        window.location.reload()

    } catch {
        return
    }
}


async function deleteCar(carId) {
    if (!confirm("Are you sure you want to delete this car?")) return;


    try {
        await fetch("/admin/car/delete/" + carId, {
            method: 'POST'
        })

        window.location.reload()

    } catch {
        return
    }
}

async function saveCar() {
    const make = document.getElementById('carMake').value
    const model = document.getElementById('carModel').value
    const year = document.getElementById('carYear').value
    const price = document.getElementById('carPrice').value
    const color = document.getElementById('carColor').value
    const description = document.getElementById('carDescription').value
    const ImageFile = document.getElementById('carImage').files[0]

    if (!make || !model || !year || !price || !color || !description || !ImageFile) {
        alert("Please fill all the inputs.");
        return;
    }

    let formData = new FormData();
    formData.append("Image", ImageFile);
    formData.append("Make", make);
    formData.append("Model", model);
    formData.append("Year", year);
    formData.append("Price", price);
    formData.append("Color", color);
    formData.append("Description", description);
    formData.append("Id", '');

    try {
        await fetch('/admin/car/create', {
            method: 'POST',
            body: formData
        })

        window.location.reload()

    } catch {
        return
    }
}



async function toggleUserAdmin(username, userId, isAdmin) {
    if (!confirm(`Are you sure you want to ${isAdmin ? 'give admin to' : 'remove admin from'} ${username}`)) return;

    let formData = new FormData();
    formData.append("UserId", userId);
    formData.append("IsAdmin", isAdmin);

    try {
        await fetch(`/admin/user/role`, {
            method: 'POST',
            body: formData
        })

        window.location.reload()

    } catch (error) {
        return
    }
}


function page(pageNumber) {
    let elements = document.getElementsByClassName("car-row")
    if (elements.length < 10) {
        alert("You are already on the last page.")
        return
    }

    window.location.href = window.location.pathname +
        `?${new URLSearchParams({ page: pageNumber }).toString()}`
}