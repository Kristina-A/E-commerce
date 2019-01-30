function ocistiModal() {
    $("#inputNameChar").val("");
    $("#inputValueChar").val("");
}


$(document).ready(function () {
    $("#deleteProduct").click(function () {
        var classes = $(this).attr("class");
        var prodID = classes.split(" ")[0];

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
        var classes = $("#editProduct").attr("class");
        var id = classes.split(" ")[0];
        var name = $("#inputEditName").val();
        var price = $("#inputEditPrice").val();
        var pictureBtn = document.getElementById("btnEditPicture");
        var picture = pictureBtn.files[0];

        var formData = new FormData();
        formData.append("id", id);
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
        var button = e.relatedTarget.id;
        ocistiModal();
        if (button == "editChar") {
            var row = e.relatedTarget.closest("tr");
            var name = $(row).find('td.name').text();
            var value = $(row).find('td.value').text();

            $("#inputNameChar").val(name);
            $("#inputValueChar").val(value);
        }
    });
})