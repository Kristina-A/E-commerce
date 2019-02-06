var toggler = document.getElementsByClassName("caret");
var i;

for (i = 0; i < toggler.length; i++) {
  toggler[i].addEventListener("click", function() {
    this.parentElement.querySelector(".nested").classList.toggle("active");
    this.classList.toggle("caret-down");
  });
}

function GetCategories() {
    $.ajax({
        type: "GET",
        url: '/Product/ReturnCategories',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
        },
        error: function () {
            alert("fail");
        }
    });
}

$(document).ready(function () {


});
