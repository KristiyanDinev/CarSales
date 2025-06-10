
function toggleForm() {
    document.getElementById("carFormContainer").style.display = "block";
    document.getElementById("formTitle").innerText = "Add New Car";
    clearForm();
}

function cancelEdit() {
    document.getElementById("carFormContainer").style.display = "none";
    clearForm();
}

function clearForm() {
    document.getElementById('carId').value = '';
    document.getElementById('carMake').value = '';
    document.getElementById('carModel').value = '';
    document.getElementById('carYear').value = '';
    document.getElementById('carPrice').value = '';
    document.getElementById('carColor').value = '';
    document.getElementById('carDescription').value = '';
    document.getElementById('carImage').value = '';
}


async function editCar(carId) {
    if (!confirm("Are you sure you want to edit this car?")) return;
}


async function deleteCar(carId) {
    if (!confirm("Are you sure you want to delete this car?")) return;

}

async function saveCar() {
    const image = document.getElementById('carImage').files[0];
    if (image) formData.append("ImageFile", image);
}