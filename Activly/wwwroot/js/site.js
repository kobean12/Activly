// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
var modal = document.getElementById("loginModal");

var btn = document.getElementById("loginBtn");

var span = document.getElementsByClassName("close")[0];

var toggleBtn = document.getElementById("toggleBtn");
var loginForm = document.getElementById("loginForm");
var registerForm = document.getElementById("registerForm");
var modalTitle = document.getElementById("modalTitle");
var toggleText = document.getElementById("toggleText");

btn.onclick = function () {
    modal.style.display = "block";
}

span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

toggleBtn.onclick = function () {
    if (loginForm.style.display === "none" || loginForm.style.display === "") {

        loginForm.style.display = "block";
        registerForm.style.display = "none";
        modalTitle.textContent = "Login";
        toggleText.innerHTML = "Don't have an account? <span id='toggleBtn' style='color: blue; cursor: pointer;'>Register here</span>";

        document.getElementById("toggleBtn").onclick = toggleForms;
    } else {
        loginForm.style.display = "none";
        registerForm.style.display = "block";
        modalTitle.textContent = "Register";

        toggleText.innerHTML = "Already have an account? <span id='toggleBtn' style='color: blue; cursor: pointer;'>Login here</span>";

        document.getElementById("toggleBtn").onclick = toggleForms;
    }
};
function toggleForms() {
    toggleBtn.onclick();
}

document.getElementById('profilePicture').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const preview = document.getElementById('preview');
            preview.src = e.target.result;
            preview.style.display = 'block'; 
        };
        reader.readAsDataURL(file);
    }
});
