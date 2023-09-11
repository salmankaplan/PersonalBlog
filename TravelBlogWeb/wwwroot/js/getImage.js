
//$(document).ready(function () {
//    $("#btnUpdate").click(function (event) {
//        event.preventDefault();

//        //var addUrl = app.Urls.categoryAddUrl;
//        //var redirectUrl = app.Urls.articleAddUrl;

//        var articleUpdateViewModel = {
//            Name: $('img[id=imageName]').val()
//        }
//        var jsonData = JSON.stringify(articleUpdateViewModel);
//        console.log(jsonData);

//        $.ajax({
//            url: addUrl,
//            type: "Get",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            data: jsonData,
//            success: function (data) {
//                setTimeout(function () {
//                    window.location.href = redirectUrl;
//                }, 1500);
//            },
//            error: function () {
//                toast.error("Bir hata oluştu.", "Hata");
//            }
//        });
//    });
//});