// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Navbar side
$("#nav-bar-toggle").click(function () {
    $("#nav-bar-side").show()
})

$("#header__nav--close-icon").click(function () {
    $("#nav-bar-side").hide()
})

//Hien thi file trong iframe
$('.material-link').click(function (e) {
    e.preventDefault(); // Prevent default link behavior
    $('#iframe-contribute').attr('src', $(this).attr("data"))

});