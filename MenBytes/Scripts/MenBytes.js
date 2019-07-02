

$(function () {



    var chatHub = $.connection.chatHub;

    console.log(chatHub);

    $(".sendMessageBtn").click(function() {
        var message = $("#msgChat").val();
        var datetime = new Date();
        var formetedDate = datetime.toLocaleDateString(); 
        var te = "<div class='row'><div class='col-lg-12'><div class='media'><a class='pull-left' href='#'><img class='media-object img-circle img-chat' src='/clientHtml/images/SiteImages/right_C.png' alt=''></a><div class='media-body'><h4 class='media-heading' style=margin-left='4px'>You :)<span class='small pull-right'>" + moment(formetedDate).format("DD-MM-YYYY") +"</span></h4><p>" + message +"</p></div></div></div></div ><hr>";

        $(".currentmsg").append(te);

        chatHub.server.newMessage("Tayyab", message);
        $("#msgChat").val("");
    });

    $(".AdminsendMessageBtn").click(function () {
        var message = $("#adminmsgChat").val();
        var datetime = new Date();
        var formetedDate = datetime.toLocaleDateString();
        var te = "<div class='row'><div class='col-lg-12'><div class='media'><a class='pull-left' href='#'><img class='media-object img-circle img-chat' src='/clientHtml/images/SiteImages/right_C.png' alt=''></a><div class='media-body'><h4 class='media-heading'> You :)<span class='small pull-right'>" + moment(formetedDate).format("DD-MM-YYYY")  + "</span></h4><p>" + message + "</p></div></div></div></div ><hr>";

        $(".currentmsg").append(te);

        chatHub.server.newMessage("Tayyab", message);

        $("#adminmsgChat").val("");

    });


    //function makeIncomingMessage(message) {
    //    var date = new Date().data;


    //    var te = "<div class='row'><div class='col-lg-12'><div class='media'><a class='pull-left' href='#'><img class='media-object img-circle img-chat' src='http://bootdey.com/img/Content/avatar/avatar6.png' alt=''></a><div class='media-body'><h4 class='media-heading'>Jane Smith<span class='small pull-right datespan'></span></h4><p class='msgtext'></p></div></div></div></div ><hr>";
    //    te.find(".msgtext").text(message);
    //    te.find(".datespan").text(date);
    //    $(".currentmsg").append(te);
    //}


    chatHub.client.sentMessage = function(user, message,date) {
        var d = new Date(date).time;
        var te = "<div class='row'><div class='col-lg-12'><div class='media'><a class='pull-left' href='#'><img class='media-object img-circle img-chat' src='/clientHtml/images/SiteImages/left_C.png' alt=''></a><div class='media-body'><h4 class='media-heading'>" + (user || 'Unknow User')+ "<span class='small pull-right'>" + moment(date).format("DD-MM-YYYY")+"</span></h4><p>" +
                message +
                "</p></div></div></div></div > <hr>";

        $(".currentmsg").append(te);
    };

    $.connection.hub.start().done(function() {

        console.log("Connected Hub");

        chatHub.server.joinRoom("Chat/Admin");
    });
});