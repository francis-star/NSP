var Address = "../";

$(function () {
    InitControl();
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [{ text: '新增', id: "search", icon: 'search', click: onToFirst },
            { line: true },
       {
           text: '取消', icon: 'logout', click: function (item) {
               parent.layer.closeAll('iframe');
           }
       }]
    });

    //$("#Province").ligerComboBox({
    //    data: "",
    //    textField: "LS_Name",
    //    valueField: "LS_Code",
    //    valueFieldID: "value5",
    //    width: 135,
    //    cancelable: false,
    //    isMultiSelect: false
    //});

    //$("#City").ligerComboBox({
    //    data: "",
    //    textField: "LS_Name",
    //    valueField: "LS_Code",
    //    valueFieldID: "value6",
    //    width: 135,
    //    cancelable: false,
    //    isMultiSelect: false
    //});
    //GetArea1();
}

function onToFirst() {
    if ($("#BL_Phone").val()=="") {
        layer.msg('客户号码不能为空！');
        return;
    }

    if ($("#BL_Comment").val() == "") {
        layer.msg('备注信息不能为空！');
        return;
    }
    //if ($("#Province").val() == "") {
    //    layer.msg('省份不能为空！');
    //    return;
    //}

    //if ($("#City").val() == "" || $("#City").val() == "全部") {
    //    layer.msg('请完善城市信息');
    //    return;
    //}

    var arr = new Array();
    arr[0] = $("#BL_Phone").val();
    arr[1] = $("#BL_Comment").val();
    arr[2] = "";//$("#value5").val();
    arr[3] = "";//$("#Province").val();
    arr[4] = "";//$("#value6").val();
    arr[5] = "";//$("#City").val();
    ReturnMethod("BLL_BlackListDts", "PlInsertBlackPhone", arr, Address, "SaveDataSuccess");
}

//省下拉框
function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
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
    Data1 = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data1.length > 0) {
        Data1.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#City").ligerComboBox({
        data: Data1,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data1[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });
}

var CustomMethod = {
    SaveDataSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('添加成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        }
        else {
            $.ligerDialog.error('号码重复！');
        }
    }
}