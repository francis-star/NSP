var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../"; //当前的html与Aspx中PubForm的文件深度
var PageIndex = 1;
var PageNum = 10;
var Code=GetQueryString("Code");

$(function () {
    InitControl();
    GetSearchStrData1();
    GetArea1();
    InitData();
});

function InitControl() {

    $("#Inp_BeginTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 180 });

    $("#Inp_EndTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 180 });

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '保存', icon: 'save', click: SaveData },
                    { line: true },
                    { text: '退出', icon: 'logout', click: Logout }
        ]
    });

    $("#MobileTerminal").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value1",
        width: 180
    });

    $("#PlatformName").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value2",
        width: 180
    });

    $("#InformationCategory").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value3",
        width: 180,
        isMultiSelect: false
    });

    $("#InformationType").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value4",
        width: 180,
        cancelable: false,
        isMultiSelect: false
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

    $("#District").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value7",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

}

//获取移动端
function GetSearchStrData1() {
    var arr = new Array();
    arr[0] = "";
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    $("#MobileTerminal").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value1",
        value: Data[1].LS_Code,
        width: 180,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
            if (newvalue != null && newvalue != "") {

                $("#PlatformName").val("");
                $("#value2").val("");

                GetSearchStrData2(newvalue);
            }
        }
    });
}

//获取平台名称
function GetSearchStrData2(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    $("#PlatformName").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value2",
        value: Data[0].LS_Code,
        width: 180,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            $("#InformationCategory").val("");
            $("#value3").val("");
            if (newvalue != null && newvalue != "") {
                GetSearchStrData3(newvalue);
            } else {
                $("#InformationCategory").ligerComboBox({
                    data: "",
                    textField: "LS_Name",
                    valueField: "LS_Code",
                    valueFieldID: "value3",
                    width: 180,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });


}

//获取类别
function GetSearchStrData3(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"LS_Code\": \"\", \"LS_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#InformationCategory").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value3",
        value: Data[0].LS_Code,
        width: 180,
        cancelable: false,
        isMultiSelect: false,
        onSelected: function (newvalue) {
            $("#InformationType").val("");
            $("#value4").val("");
            if (newvalue != null && newvalue != "") {
                GetSearchStrData4(newvalue);
            } else {
                $("#InformationType").ligerComboBox({
                    data: "",
                    textField: "LS_Name",
                    valueField: "LS_Code",
                    valueFieldID: "value4",
                    width: 180,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

//获取类别
function GetSearchStrData4(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"LS_Code\": \"\", \"LS_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#InformationType").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value4",
        value: Data[0].LS_Code,
        width: 180,
        cancelable: false,
        isMultiSelect: false
    });
}

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
        value: Data[1].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
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
                $("#District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetArea2(DataCode) {
    PageIndex = 1;
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
        cancelable: false,
        onSelected: function (newvalue) {
            $("#Inp_District").val("");
            $("#value7").val("");
            if (newvalue != null && newvalue != "") {
                GetArea3(newvalue);
            } else {
                $("#District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetArea3(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = '2';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#District").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value7",
        value: Data[0].SA_Code,
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });
}

function InitData() {    
    GetData();
}

function GetData() {
    if (Code != "") {
        var arr = new Array();
        arr[0] = Code;
        var Data = eval(ReturnValue("BLL_PublicInfoDts", "GetPublicInfo", arr, Address));
        if (Data.length > 0) {
            $("#MobileTerminal").ligerComboBox({
                value: Data[0].Pub_LS_Code1,
            });
            $("#PlatformName").ligerComboBox({
                value: Data[0].Pub_LS_Code2,
            });
            $("#InformationCategory").ligerComboBox({
                value: Data[0].Pub_LS_Code3,
            });
            $("#InformationType").ligerComboBox({
                value: Data[0].Pub_LS_Code4,
            });
            $("#Province").ligerComboBox({
                value: Data[0].Pub_SA_Code1,
            });
            $("#City").ligerComboBox({
                value: Data[0].Pub_SA_Code2,
            });
            $("#District").ligerComboBox({
                value: Data[0].Pub_SA_Code3,
            });
            $("#Inp_Title").val(Data[0].Pub_Title);
            $("#PicPath1").val(Data[0].Pub_Pic1);
            $("#PicPath2").val(Data[0].Pub_Pic2);
            $("#PicPath3").val(Data[0].Pub_Pic3);

            $("#txtContent").val(unescape(Data[0].Pub_Content));
            $("#Inp_ComeFrom").val(Data[0].Pub_ArticleSource);
            $("#KeyWords").val(Data[0].Pub_KeyWords);
        }
    }
}

function SaveData() {
    var Inp_Title = $("#Inp_Title").val();
    var Inp_ComeFrom = $("#Inp_ComeFrom").val();
    if (Inp_Title.length > 50) {
        $.ligerDialog.warn("标题长度不能超过50个字!");
        return false;
    }
    if (Inp_ComeFrom.length > 20) {
        $.ligerDialog.warn("文章出处长度不能超过20个字!");
        return false;
    }
    if (Inp_Title.length == 0) {
        $.ligerDialog.warn("标题长度不能为空!");
        return false;
    }
    if (Inp_ComeFrom.length == 0) {
        $.ligerDialog.warn("文章出处长度不能为空!");
        return false;
    }

    var arr = new Array();
    arr[0] = $("#value1").val();
    arr[1] = $("#value2").val();
    arr[2] = $("#value3").val();
    arr[3] = $("#value4").val();
    arr[4] = ""
    arr[5] = $("#value5").val();
    arr[6] = $("#value6").val();
    arr[7] = $("#value7").val();
    arr[8] = $("#Inp_Title").val();
    arr[9] = $("#PicPath1").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28:8080');
    arr[10] = $("#PicPath2").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28:8080');
    arr[11] = $("#PicPath3").val().replace('hc.njhcsoft.com/HcUpFile', '211.149.239.28:8080');
    arr[12] = escape($("#txtContent").val());
    arr[13] = $("#Inp_ComeFrom").val();
    arr[14] = $("#KeyWords").val();
    arr[15] = Code;
    arr[16] = $("#Province").val();
    arr[17] = $("#City").val();
    arr[18] = $("#District").val();
    if (Code == "" || Code == "null" || Code == null) {
        ReturnMethod("BLL_PublicInfoDts", "Update", arr, Address, "InsertSuccess");
    } else {
        ReturnMethod("BLL_PublicInfoDts", "Update", arr, Address, "UpdateSuccess");
    }
}

function Logout() {
    parent.layer.closeAll('iframe');
}

//回调刷新列表页
function End() {
    window.close();
    var pWindow = window.dialogArguments; if (pWindow != null) {
        alert(parent);
        parent.pWindow.InitData();

    } else {
        window.opener.InitData();
    }
}

var CustomMethod = {
    InsertSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('添加成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        } else {
            $.ligerDialog.error("添加失败!");
        }
    },
    UpdateSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('修改成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        } else {
            $.ligerDialog.error("修改失败!");
        }
    }
}

function escapeHTML(str) {
    str = String(str).replace(/&/g, '&amp;').
      replace(/>/g, '&gt;').
      replace(/</g, '&lt;').
      replace(/"/g, '&quot;');
    return str;
}
function unescapeHTML(str) {
    str = String(str).replace(/&gt;/g, '>').
      replace(/&lt;/g, '<').
      replace(/&quot;/g, '"').
      replace(/&amp;/g, '&');
    return str;
}