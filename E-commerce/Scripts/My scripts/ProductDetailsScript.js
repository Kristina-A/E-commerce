function ocistiModal() {
    $("#inputNameChar").val("");
    $("#inputValueChar").val("");
}

function addComment(name, surname, comment, role) {
    var div = document.getElementById("comments");

    var commentDiv = document.createElement("div");

    var labelUser = document.createElement("label");
    labelUser.setAttribute("class", "col-sm-7");
    labelUser.style.color = "blue";
    labelUser.innerHTML = name + " " + surname;

    var labelContent = document.createElement("label");
    labelContent.setAttribute("class", "col-sm-7");
    labelContent.innerHTML = comment;

    var labelReply;
    if (role == "Admin") {
        labelReply = document.createElement("label");
        labelReply.setAttribute("class", "col-sm-7");
        labelReply.innerHTML = "odgovor";
        labelReply.style.marginLeft = "50px";
        labelReply.style.color = "blue";
    }

    commentDiv.appendChild(labelUser);
    commentDiv.appendChild(labelContent);
    if(role=="Admin")
        commentDiv.appendChild(labelReply);
    div.appendChild(commentDiv);
}

function addReview(name, surname, grade, comment) {
    var div = document.getElementById("reviews");

    var reviewDiv = document.createElement("div");

    var labelUser = document.createElement("label");
    labelUser.setAttribute("class", "col-sm-4");
    labelUser.style.color = "blue";
    labelUser.innerHTML = name + " " + surname;

    var labelGrade = document.createElement("label");
    labelGrade.setAttribute("class", "col-sm-6");
    labelGrade.innerHTML = grade;

    var star = document.createElement("i");
    star.setAttribute("class", "fas fa-star");

    var labelComment = document.createElement("label");
    labelComment.setAttribute("class", "col-sm-6");
    labelComment.innerHTML = comment;

    reviewDiv.appendChild(labelUser);
    labelGrade.appendChild(star);
    reviewDiv.appendChild(labelGrade);
    reviewDiv.appendChild(labelComment);
    div.appendChild(reviewDiv);
}


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

    $("#showReviews").on("click", function () {
        if ($(this).attr("class") != "disabled") {
            $.ajax({
                type: "POST",
                url: '/Product/GetReviews',
                data: { "id": prodID },
                success: function (data) {
                    var num = data.number;
                    for (var i = 0; i < num; i++) {
                        addReview(data.people[i]["Name"], data.people[i]["Surname"], data.revs[i]["Grade"], data.revs[i]["Comment"]);
                    }
                    $("#showReviews").addClass("disabled");
                },
                error: function () {
                    alert("fail");
                }
            });
        }
    });

    $("#showReviews").on({
        mouseenter: function () {
            $(this).css("color", "red");
            $(this).css("cursor", "pointer");
        },
        mouseleave: function () {
            $(this).css("color", "black");
        }
    });

    $("#showComments").on({
        mouseenter: function () {
            $(this).css("color", "red");
            $(this).css("cursor", "pointer");
        },
        mouseleave: function () {
            $(this).css("color", "black");
        }
    });

    $("#showComments").on("click", function () {
        if ($(this).attr("class") != "disabled") {
            $.ajax({
                type: "POST",
                url: '/Product/GetComments',
                data: { "id": prodID },
                success: function (data) {
                    var num = data.number;
                    for (var i = 0; i < num; i++) {
                        var role = data.status;
                        addComment(data.people[i]["Name"], data.people[i]["Surname"], data.com[i]["Content"], role);
                    }
                    $("#showComments").addClass("disabled");
                },
                error: function () {
                    alert("fail");
                }
            });
        }
    });

    $("#postComment").on("click", function () {
        var txt = $("#comment").val();

        $.ajax({
            type: "POST",
            url: '/Product/AddComment',
            data: { "prodId": prodID, "content":txt },
            success: function () {
                window.location.href = '/Product/ProductDetails/' + prodID;
            },
            error: function () {
                alert("fail");
            }
        });
    });

    $(".far.fa-star").on({
        mouseenter: function () {
            for (var i = 1; i <= $(this).attr("id");i++)
                $("#"+i).attr("class", "fas fa-star");
            $(this).css("cursor", "pointer");
        },
        mouseleave: function () {
            for (var i = 1; i <= $(this).attr("id"); i++)
                $("#" + i).attr("class", "far fa-star");
        }
    });

    $("#discardReview").on("click", function () {
        $("#reviewModal").modal("toggle");
    });

    $("#reviewModal").on("show.bs.modal", function (e) {
        var star = e.relatedTarget.id;
        $("#inputComment").val("");
        $("#lblStar").text(star);
    });

    $("#saveReview").on("click", function () {
        var star = $("#lblStar").text();
        var comm = $("#inputComment").val();

        $.ajax({
            type: "POST",
            url: '/Product/AddReview',
            data: { "id": prodID, "grade": star, "comment": comm },
            success: function () {
                $("#reviewModal").modal("toggle");
                window.location.href = '/Product/ProductDetails/' + prodID;
            },
            error: function () {
                alert("fail");
            }
        });
    });
})