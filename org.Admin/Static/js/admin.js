$(function () {
    $("#event1").on("click", function () {
        $('#contentModal')
            .modal({
                closable: false,
                onDeny: function () {
                    return true;
                },
                onApprove: function () {
                    UpdateContent();
                    return false;
                }
            })
            .modal('show');

    });
    $("#event2").on("click", function () {
        $("#stateMsg").hide();
        $('#stateModal')
            .modal({
                closable: false,
                 onDeny: function () {
                    return true;
                },
                onApprove: function () {
                    UpdateTopicState();
                    return false;
                }
            })
            .modal('show');
    });
    $("#event3").on("click", function () {
        $('#redpackModal')
            .modal({
                closable: false,
                 onDeny: function () {
                    return true;
                },
                onApprove: function () {
                    UpdateBingo();
                    return false;
                }
            })
            .modal('show');
    });

});


function GetQiList() {
    var fid = $("#fid").val();
    var qilist = $("#qilist");
    qilist.empty();
    qilist.append("<option value=\"0\">==请选择期数==</option>");
    if (fid > 0) {
        $.ajax({
            url: "/ajax/getqilist",
            data: { fid: fid },
            type: "get",
            dataType: "json",
            success: function (d) {
                if (d.length > 0) {
                    d.map(function (item) {
                        qilist.append("<option value=\"" + item.qi + "\">" + item.qi + "</option>");
                    });
                }
            },
            error: function (d) {
                alert(d.responseText);
            },
        });
    }
}

function SetQi() {
    var qilist = $("#qilist");
    if (qilist.val() > 0) {
        $("#qi").val(qilist.val());
    }
}


function Msg(obj,msg) {
    obj.show();
    obj.find("p").html(msg);
}


function num() {
    var i = Math.random() * 100000;
    var a = "" + i + "";
    a = a.substr(0, 5);
    return a
}

function reloadFrame() {
    $('#detail').attr('src', $('#detail').attr('src'));
}

//删除
function Del(url, id) {
    var t = confirm("请确认删除！");
    if (t) {
        $.post(url, { id: id },
            function (data) {
                if (data > 0) {
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                }
                else {
                    alert("删除失败，请重试！");
                }
            });
    }
}



function Login() {
    var QQnumber = $("#QQnumber").val();
    var code = $("#code").val();
    var password = $("#password").val();
    var msg = $("#msg");

    if (QQnumber.length == 0 || QQnumber == "") {
        msg.text("帐号不能为空");
        return false;
    }
    if (password.length == 0 || password == "") {
        msg.text("密码不能为空");
        return false;
    }
    if (code.length == 0 || code == "") {
        msg.text("验证码不能为空");
        return false;
    }
    $.ajax({
        url: "/login/check",
        data: {
            qqnum: QQnumber,
            password: password,
            code: code
        },
        type: "post",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.code > 0) {
                top.location.href = "/";
            } else {
                msg.text(data.msg);
            }
        },
        error: function (data) {
            alert("请求异常");
        }
    });

    return false;
}

