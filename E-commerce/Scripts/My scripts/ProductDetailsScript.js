function ocistiModal() {
    $("#inputNameChar").val("");
    $("#inputValueChar").val("");
}

//function addRow(name, value) {
//    var body = document.getElementById("charTable").getElementsByTagName("tbody")[0];

//    var newRow = document.createElement("tr");

//    var dataName = document.createElement("td");
//    dataName.setAttribute("class", "name");
//    dataName.innerHTML = name;

//    var dataValue = document.createElement("td");
//    dataValue.setAttribute("class", "value");
//    dataValue.innerHTML = value;

//    var dataButton = document.createElement("td");
//    var button = document.getElementById("editChar");

//    dataButton.appendChild(button);
//    newRow.appendChild(dataName);
//    newRow.appendChild(dataValue);
//    newRow.appendChild(dataButton);
//    body.appendChild(newRow);
//}


$(document).ready(function () {
    var button;
    var row;
    var classes = $("#pictureBox").attr("class");//mora za ovo, treba nesto sto je svim userima i adminima dostupno
    var prodID = classes.split(" ")[0];

    $.ajax({
        type: "POST",
        url: '/Product/AverageGrade',
        data: { "id": prodID },
        success: function (data) {
            var text = $("#lblGrade").text();
            $("#lblGrade").text(text + " (" + data.number + "): " + data.grade);
        },
        error: function () {
            alert("fail");
        }
    });

    $("#deleteProduct").click(function () {
        //var classes = $(this).attr("class");
        //var prodID = classes.split(" ")[0];

        $.ajax({
            type: "POST",
            url: '/Product/DeleteProduct',
            data: { "id": prodID },
            success: function () {
                window.location.href = '/Home/Index';
            },
            error: function () {
                alert("fail");
            }
        });
    });

    $("#discardEditing").click(function () {
        $("#editModal").modal("toggle");
    });

    $("#saveEditing").click(function () {
        //var classes = $("#editProduct").attr("class");
        //var id = classes.split(" ")[0];
        var name = $("#inputEditName").val();
        var price = $("#inputEditPrice").val();
        var pictureBtn = document.getElementById("btnEditPicture");
        var picture = pictureBtn.files[0];

        var formData = new FormData();
        formData.append("id", prodID);
        formData.append("name", name);
        formData.append("price", price);
        formData.append("picture", picture);

        $.ajax({
            type: "POST",
            url: '/Product/EditProduct',
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: function () {
                $("#editModal").modal("toggle");
                window.location.href = '/Product/ProductDetails/'+id;
            },
            error: function () {
                alert("fail");
            }
        });
    });

    $("#discardChar").on("click", function () {
        $("#characteristicsModal").modal("toggle");
    });

    $("#characteristicsModal").on("show.bs.modal", function (e) {
        button = e.relatedTarget.id;
        ocistiModal();
        if (button == "editChar") {
            row = e.relatedTarget.closest("tr");
            var name = $(row).find('td.name').text();
            var value = $(row).find('td.value').text();

            $("#inputNameChar").val(name);
            $("#inputValueChar").val(value);
        }
    });

    $("#saveChar").on("click", function () {
        //var classes = $("#addChar").attr("class");
        //var prodID = classes.split(" ")[0];
        var name = $("#inputNameChar").val();
        var value = $("#inputValueChar").val();
        var oldName = null;
        var oldVal = null;
        if (button == "editChar") {
            oldName = $(row).find('td.name').text();
            oldVal = $(row).find('td.value').text();
        }

        $.ajax({
            type: "POST",
            url: '/Product/EditCharacteristics',
            data: { "id": prodID, "charName": name, "charValue": value, "oldN": oldName, "oldV": oldVal },
            success: function () {
                $("#characteristicsModal").modal("toggle");
                if (button == "editChar") {
                    $(row).find('td.name').text(name);
                    $(row).find('td.value').text(value);
                }
                else {
                    window.location.href = '/Product/ProductDetails/' + prodID;
                }
            },
            error: function () {
                alert("fail");
            }
        });
    });
})