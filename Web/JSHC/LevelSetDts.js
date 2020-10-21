var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../"; //当前的html与Aspx中PubForm的文件深度
var Ptype = GetQueryString("Ptype");//Ptype:PageType,页面类型,0:新增页面,1:修改页面
var Code = GetQueryString("Code");
var PCode = GetQueryString("PCode");
var HasData = "0";
var ligerComboBoxs = "";
var data1;


$(function () {

    InitControl();
    InitData();

});

function InitControl() {

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '保存', id: 'btnAdd', icon: 'save', click: SaveData }
        ]
    });

    //var Data = eval(ReturnValue("BLL_LevelSetDts", "GetESysModule", "", Address));

    //$("#LevelSetType").ligerComboBox({
    //    data: Data,
    //    textField: "SM_Name",
    //    valueField: "SM_Code",
    //    valueFieldID: "value",
    //    width: 180
    //});

}

function InitData() {
    var arr = new Array();
    arr[0] = Code;
    var Data = eval(ReturnValue("BLL_LevelSetDts", "GetLevelSet", arr, Address));
    if (Data.length > 0) {
        if (Ptype == "0") {
            if (Data[0].LS_PName != "网页" && Data[0].LS_PName != "微信" && Data[0].LS_PName != "APP") {
                
                HasData = "0";
            } else {
                //$("#LevelSetType").ligerComboBox({
                //    value: "1018"
                //});

                HasData = "1";
            }
            $("#LevelSetPName").val(Data[0].LS_Name);

            //获取可使用者
            var arr1 = new Array();
            arr1[0] = Code;
            data1 = eval(ReturnValue("BLL_LevelSetDts", "GetUsers", arr1, Address));
            ligerComboBoxs = $("#LevelSetUsers").ligerComboBox({
                width: 180,
                selectBoxWidth: 180,
                selectBoxHeight: 120,
                textField: 'UserName', valueField: 'UserCode',
                isMultiSelect: true,
                data: data1
            });
        }
        if (Ptype == "1") {


            if (Data[0].LS_Type == "" || Data[0].LS_Type == "null" || Data[0].LS_Type == null) {
                
                HasData = "0";
            } else {

                //$("#LevelSetType").ligerComboBox({
                //    value: Data[0].LS_Type
                //});

                HasData = "1";
            }

            $("#LevelSetPName").val(Data[0].LS_PName);
            $("#LevelSetName").val(Data[0].LS_Name);

            //获取可使用者
            var arr1 = new Array();
            arr1[0] = PCode;
            data1 = eval(ReturnValue("BLL_LevelSetDts", "GetUsers", arr1, Address));
            ligerComboBoxs = $("#LevelSetUsers").ligerComboBox({
                width: 180,
                selectBoxWidth: 180,
                selectBoxHeight: 120,
                textField: 'UserName', valueField: 'UserCode',
                isMultiSelect: true,
                data: data1
            });
            ligerComboBoxs.selectValue(Data[0].LS_User_Code);

        }
    }

}

function SaveData() {

    var LevelSetName = $("#LevelSetName").val();
    if (LevelSetName.length > 50) {
        $.ligerDialog.warn("菜单名称不能超过50个字!");
        return false;
    }

    if (Ptype == "0") {

        //if (HasData == "1") {
        //    var ValuesStr = $("#value").val();
        //    if (ValuesStr == "") {
        //        $.ligerDialog.warn("类别不能为空!");
        //        return false;
        //    }
        //}

        var InsertArr = new Array();
        InsertArr[0] = Code;
        InsertArr[1] = $("#LevelSetName").val();
        InsertArr[2] = $("#value").val();
        if ($("#LevelSetUsers_val").val() != "" && $("#LevelSetUsers_val").val() != null) {
            InsertArr[3] = $("#LevelSetUsers_val").val();
        } else {
            if (data1.length > 0) {
                var LevelSetUsersStr = "";
                for (var i1 = 0; i1 < data1.length; i1++) {
                    if (i1 == 0)
                        LevelSetUsersStr += data1[i1].UserCode;
                    else
                        LevelSetUsersStr += ";" + data1[i1].UserCode;
                }
                InsertArr[3] = LevelSetUsersStr;
            } else {
                InsertArr[3] = "";
            }
        }
        //InsertArr[3] = "";
        var ResultStr = ReturnValue("BLL_LevelSetDts", "InsertLevelSet", InsertArr, Address);
        if (ResultStr == "true") {
            $.ligerDialog.success('添加成功!', '提示', function () {
                parent.location.reload();
            });
        }
        else if (ResultStr == "false") {
            $.ligerDialog.error("添加失败!");
        } else {
            $.ligerDialog.warn("节点名称重复!");
        }
    }
    if (Ptype == "1") {

        
        var UpdateArr = new Array();
        UpdateArr[0] = Code;
        UpdateArr[1] = $("#LevelSetName").val();
        UpdateArr[2] = $("#value").val();
        if ($("#LevelSetUsers_val").val() != "" && $("#LevelSetUsers_val").val() != null) {
            UpdateArr[3] = $("#LevelSetUsers_val").val();
        } else {
            if (data1.length > 0) {
                var LevelSetUsersStr = "";
                for (var i2 = 0; i2 < data1.length; i2++) {
                    if (i2 == 0)
                        LevelSetUsersStr += data1[i2].UserCode;
                    else
                        LevelSetUsersStr += ";" + data1[i2].UserCode;
                }
                UpdateArr[3] = LevelSetUsersStr;
            } else {
                UpdateArr[3] = "";
            }
        }
        //UpdateArr[3] = "";
        var ResultStrs = ReturnValue("BLL_LevelSetDts", "UpdateLevelSet", UpdateArr, Address);
        if (ResultStrs == "true") {
            $.ligerDialog.success('修改成功!', '提示', function () {
                parent.location.reload();
            });
        }
        else if (ResultStrs == "false") {
            $.ligerDialog.error("修改失败!");
        } else {
            $.ligerDialog.warn("节点名称重复!");
        }
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