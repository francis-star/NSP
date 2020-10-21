var Address = "../../"; //当前的html与Aspx中PubForm的文件深度

$(function () {
    InitControl();
});

function InitControl() {
    $("#toptoolbar").ligerToolBar({
        items: [
            { text: '保存', icon: 'save', click: SaveBillList }
        ]
    });
}

function UpFile() {
    if ($("#tbPC_Name").next().val() == "上传") {
        art.dialog.data('Path', 'tbPC_Name'); //接收返回路径
        art.dialog.data('FileType', '*.xls'); //文件类型
        art.dialog.data('Filedata', '2'); //文件大小，单位M
        OpenForm("../../UpFile/UpFile.htm", "上传文件", "500px", "150px"); //窗体大小
    }
    else {
        $("#tbPC_Name").val("");
        $("#hdFileName").val("");
        $("#tbPC_Name").next().val("上传");
    }
}

//导入数据
function SaveBillList() {
    if (validata()) {
        var arr = new Array();
        arr[0] = $("#tbPC_Name").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28:8080');//'hc.njhcsoft.com/HcUpFileTest');
        arr[1] = $("#txtRemark").val();
        arr[2] = $("#hdFileName").val();//原始文件

        ReturnMethod("BLL_CallCenter", "CheckColumns", arr, Address, "SaveDataSuccess");
    }
} 

var CustomMethod = {
    SaveDataSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('保存成功！', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        }
        else {
            $.ligerDialog.warn(text);
            $("#tbPC_Name").val("");
            $("#tbPC_Name").next().val("上传");
        }
    }
} 

function validata() {
    if ($("#tbPC_Name").val() == "") {
        $.ligerDialog.warn('请补填写完整信息！');
        $("#CA_SP_FJ1").focus();
        return false;
    }
    return true;
}