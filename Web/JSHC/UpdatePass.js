//////////////////////////////////////////////////////////////////////////////
//模块名：密码修改
//开发者：赵虎 
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
var Address = "../";
$(function () {
    InitControl();
});
//初始化控件
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                { text: '保存', icon: 'save', click: Save },
                { line: true },
                { text: '退出', icon: 'logout', click: Close }
        ]
    });
}
//保存密码
function Save() {
    if (validate()) {
        var arr = new Array();
        arr[0] = $("#tbNewPassWord").val();
        ReturnMethod("BLL_UpdatePass", "SavePass", arr, Address, "SaveDataSuccess");
    }
}

function Close() {
    parent.layer.closeAll('iframe');
}

//信息输入判定
function validate() {
    if ($("#tbOldPassWord").val() == "") {
        $.ligerDialog.warn('请输入旧密码！');
        return false;
    }
    if ($("#tbNewPassWord").val() == "") {
        $.ligerDialog.warn('请输入新密码！');
        return false;
    }
    if ($("#tbNewPassWord").val().length > 50) {
        $.ligerDialog.warn('密码 最大长度不能超过50！');
        return false;
    }
    if ($("#tbConfirmPassWord").val() == "") {
        $.ligerDialog.warn('请输入确认新密码！');
        return false;
    }
    if ($("#tbNewPassWord").val() != $("#tbConfirmPassWord").val()) {
        $.ligerDialog.warn('密码不一致！');
        return false;
    }
    var arr = new Array();
    arr[0] = $("#tbOldPassWord").val();
    var data = ReturnValue("BLL_UpdatePass", "GetPass", arr, Address);
    if (data != "true") {
        $.ligerDialog.warn('旧密码错误！');
        return false;
    }
    return true;
}

var CustomMethod = {
    SaveDataSuccess: function (text) {
        if (text == "true") {
            //$.ligerDialog.success('密码修改成功！');
            //parent.layer.closeAll('iframe');

            layer.msg('密码修改成功', { time: 1500 }, function () {
                parent.layer.closeAll('iframe');
            });
        }
        else {
            $.ligerDialog.warn('密码修改失败！');
        }
    }
}
