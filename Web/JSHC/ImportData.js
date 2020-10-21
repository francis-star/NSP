var Address = "../"; //当前的html与Aspx中PubForm的文件深度

$(function () {
    InitControl();
});

function InitControl() {

    $("#ProDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 163 });
    $("#ProDate").ligerGetDateEditorManager().setValue((new Date()).format("yyyy-MM-dd"));
    $("#toptoolbar").ligerToolBar({
        items: [
            { text: '保存', icon: 'save-disabled', click: SaveOrganization }
        ]
    });
    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    $("#City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });
    GetArea1();
}

//省下拉框
function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全国\"}]")[0]);//追加全国选项
    }
    $("#Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            if (newvalue != null && newvalue != "") {
                $("#Inp_City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#City").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

//市下拉框
function GetArea2(DataCode) {
    var arr = new Array();
    arr[0] = '1';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });
}

function UpFile() {
    art.dialog.data('Path', 'tbPC_Name'); //接收返回路径
    art.dialog.data('FileType', '*.txt'); //文件类型
    art.dialog.data('Filedata', '100'); //文件大小，单位M
    OpenForm("../UpFile/UpFile.htm", "上传文件", "500px", "150px"); //窗体大小
}
function setDefaultPricie(o) {
    var province = $(o).val();
    if (province == "河南省") {
        $("#Billing").val("20");
    }
}
//导入数据
function SaveOrganization() {
    if (validata()) {
        var arr = new Array();
        //arr[0] = $("#tbPC_Name").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28');
        arr[0] = $("#tbPC_Name").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28:8080');//'hc.njhcsoft.com/HcUpFileTest');
        arr[1] = $("#Provider").val();
        arr[2] = $("#ProDate").val();
        arr[3] = $("#Province").ligerComboBox().getValue();
        arr[4] = $("#City").ligerComboBox().getValue();
        arr[5] = $("#Billing").val();
        arr[6] = "CXT";

        var result = ReturnValue("BLL_OriginalData", "CheckColumns", arr, Address);
        if (result == "true") {
            arr[0] = arr[0].substr(arr[0].lastIndexOf('FileUp/') + 7); //获取文件名
            ReturnMethod("BLL_OriginalData", "PlImportOrignData", arr, Address, "SaveDataSuccess");
        }
        else {
            var strValue = result.split('|');
            if (strValue.length > 1) {
                if (strValue[0] == "special") {
                    $.ligerDialog.warn("筛选数据在第" + strValue[1] + "行【" + strValue[2] + "】出现错误，可能含有（英文双引号或者英文逗号特殊字符）,请检查后重新上传！");
                    $("#tbPC_Name").val("");
                } else if (strValue[0] == "contentLen") {
                    $.ligerDialog.warn("筛选数据在第" + strValue[1] + "行【" + strValue[2] + "】出现错误，指定行生成的制表符可能不符合规范,请检查后重新上传！");
                    $("#tbPC_Name").val("");
                } else {
                    $.ligerDialog.warn("筛选数据在第" + strValue[1] + "行【" + strValue[2] + "】出现错误，区号不正确！");
                    $("#tbPC_Name").val("");
                }
            } else {
                $.ligerDialog.warn('数据筛选模版错误，请重新检查后上传！');
                $("#tbPC_Name").val("");
            }
        }
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
            $.ligerDialog.error('保存失败！');
        }
    }
}

function validata() {
    if ($("#tbPC_Name").val() == "") {
        $.ligerDialog.warn('请补填写完整信息！');
        $("#CA_SP_FJ1").focus();
        return false;
    }
    else if ($("#Provider").val() == "") {
        $.ligerDialog.warn('请补填写完整信息！');
        $("#Provider").focus();
        return false;
    }
    else if ($("#ProDate").val() == "") {
        $.ligerDialog.warn('请补填写完整信息！');
        $("#ProDate").focus();
        return false;
    }
    else if ($("#City").ligerComboBox().getValue() == "") {
        $.ligerDialog.warn('请补填写完整信息！');
        $("#City").focus();
        return false;
    }
    return true;
}