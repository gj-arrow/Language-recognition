function deselect(e) {
    e.removeClass('selected');
    $("#popup").hide();
}

$(document).ready(function () {
    xOffset = 10;
    yOffset = 30;
    $('#sbmt').bind('blur click', function () {
        editTxt('#input');
        $('span').hover(function (e) {
            var text = $(this).text();
            $(this).addClass('hlight');
            window.initHandler = setTimeout(handler, 300, e, text);
            function handler(e, text) {
                $.ajax("/Home/Translator?word=" + text, {
                    success: function (data) {
                        var str = "";
                        for (var item in data) {
                            str += "<li>" + item + ':' + data[item] + "</li>";
                        }
                        $('#output').html(str)
                        $("#popup")
                            .css("top", (e.pageY - xOffset) + "px")
                            .css("left", (e.pageX + yOffset) + "px")
                            .fadeIn("fast");
                    }
                });
            }
        }, function () {
            clearTimeout(window.initHandler)
        });
        $('span').on('mouseout', function () {
            $(this).removeClass('hlight');
            deselect($('#contact'));
            $.ajax("/Home/GetTopUsers", {
                success: function (data) {
                    $('#topUsers').html(data)
                }
            });
        });
    });

    $("#clear").click(function () {
        $('#input').empty();
        deselect($('#contact'));
    });


    function editTxt(selector) {
        $(function () {
            var newHtml = '';
            var words = $(selector).text().split(/[ ,.:;\u00a0]/);
            for (i = 0; i < words.length; i++) {
                if (words[i].length >= 4)
                    newHtml += '<span class="tooltip" id=' + words[i].trim() + '>' + words[i].trim() + '</span> ';
                else if (words[i].length < 4 && words[i] != "")
                    newHtml += words[i].trim() + ' ';
            }
            $(selector).html(newHtml);
        });
    }
});