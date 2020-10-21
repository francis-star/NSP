var Address = "../../"; //当前的html与Aspx中PubForm的文件深度

$(function () {
    InitControl();
});

function InitControl() {
    $("#toptoolbar").ligerToolBar({
        items: [
            { text: '删除', icon: 'delete', click: SaveOrganization }
        ]
    });
}

function UpFile() {
    if ($("#CA_SP_FJ1").next().val() == "上传") {
        art.dialog.data('Path', 'CA_SP_FJ1'); //接收返回路径
        art.dialog.data('FileType', '*.txt'); //文件类型
        art.dialog.data('Filedata', '100'); //文件大小，单位M
        OpenForm("../../UpFile/UpFile.htm", "上传文件", "500px", "150px"); //窗体大小
    }
    else {
        $("#CA_SP_FJ1").val("");
        $("#CA_SP_FJ1").next().val("上传");
    }
}

//导入数据
function SaveOrganization() {
    var arr = new Array();
    //arr[0] = $("#CA_SP_FJ1").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28');
    arr[0] = $("#CA_SP_FJ1").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28:8080');//'hc.njhcsoft.com/HcUpFileTest');
    if (arr[0] == "") {
        $.ligerDialog.error('请先上传文件！');
        return;
    }
    var result = ReturnValue("BLL_OriginalData", "CheckXFBColumns", arr, Address);
    if (result == "true") {
        arr[0] = arr[0].substr(arr[0].indexOf('FileUp') + 7); //获取文件名
        arr[1] = "XFB";
        ReturnMethod("BLL_OriginalData", "InvalidData", arr, Address, "SaveDataSuccess");
    }
    else {
        var strValue = result.split('|');
        if (strValue.length > 1) {
            if (strValue[0] == "special") {
                $.ligerDialog.warn("筛选数据在第" + strValue[1] + "行【" + strValue[2] + "】出现错误，可能含有（英文双引号或者英文逗号特殊字符）,请检查后重新上传！");
                $("#CA_SP_FJ1").val("");
            } else if (strValue[0] == "contentLen") {
                $.ligerDialog.warn("筛选数据在第" + strValue[1] + "行【" + strValue[2] + "】出现错误，指定行生成的制表符可能不符合规范,请检查后重新上传！");
                $("#CA_SP_FJ1").val("");
            } else {
                $.ligerDialog.warn("筛选数据在第" + strValue[1] + "行【" + strValue[2] + "】出现错误，手机号格式不正确！");
                $("#CA_SP_FJ1").val("");
            }
        } else {
            $.ligerDialog.warn('数据筛选模版错误，请重新检查后上传！');
            $("#CA_SP_FJ1").val("");
        }

        $("#CA_SP_FJ1").next().val("上传");
    }
}

var CustomMethod = {
    SaveDataSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('删除成功！', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        }
        else {
            $.ligerDialog.error('删除失败！');
        }
    }
}