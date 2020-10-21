var Address = "../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;
$(function () {
    InitControl();
    InitData();
});
//初始化控件
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', icon: 'search', click: onToFirst },
            { line: true },
            {
                text: '新建', id: 'btnAdd', icon: 'add', click: function () {
                    top.layer.open({
                        type: 2,
                        title: '新增用户信息',
                        shadeClose: false,
                        shade: 0.3,
                        area: ['60%', '70%'],
                        content: '../Form/frm_UserSetDts.htm', //iframe的url
                        end: function () {
                            Search();
                        }
                    });
                }
            },
            { line: true },
            {
                text: '修改', id: 'btnEdit', icon: 'modify', click: function () {
                    var row = gridManager.getSelected();
                    if (row) {
                        top.layer.open({
                            type: 2,
                            title: '修改用户信息',
                            shadeClose: false,
                            shade: 0.3,
                            area: ['60%', '70%'],
                            content: '../Form/frm_UserSetDts.htm?UserCode=' + row.User_Code, //iframe的url
                            end: function () {
                                Search();
                            }
                        });
                    }
                    else {
                        $.ligerDialog.warn('请选择要修改的用户！'); return;
                    }
                }
            },
            { line: true },
            { text: '删除', id: 'btnDelete', icon: 'delete', click: Delete }
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311940");

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '姓名', name: 'User_Name', minWidth: 90 },
            { display: '用户名', name: 'User_LoginName', minWidth: 90 },
            //{ display: '密码', name: 'User_PassWord', minWidth: 90 },
            { display: '性别', name: 'User_Sex', minWidth: 90 },
            { display: '工作地点', name: 'User_Place', minWidth: 90 },
            { display: '年龄', name: 'User_Age', minWidth: 90 },
            { display: '手机号码', name: 'User_Phone', minWidth: 90 },
            { display: '职务', name: 'User_Post', minWidth: 90 },
            { display: '入职时间', name: 'User_EntryDate', minWidth: 90 }
        ],
        pageSize: 20, width: '98%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });
}
function InitData() {
    var Data = eval(ReturnValue("BLL_CustomerReturns", "GetUserPlace", "", Address));

    $("#tbPlace").ligerComboBox({
        data: Data,
        textField: "User_Place",
        valueField: "User_Place",
        valueFieldID: "value1",
        value: Data[0].User_Place,
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });
    liger.get("tbPlace").setText("全部");
    Search();
}
//查询
function Search() {
    var arr = new Array();
    arr[0] = $("#Cust_Name").val();
    arr[1] = PageIndex;
    arr[2] = PageNum;
    arr[3] = $("#tbPlace").ligerComboBox().getValue();

    MaxCount = ReturnValue("BLL_UserSet", "GetNum", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetUser";
    var BLL = "BLL_UserSet";
    gridManager = $("#grid").ligerGrid({
        url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });
    return false;
}

function onToFirst() {
    PageIndex = 1;
    Search();
    return false;
}
//末页
function onToLast() {
    PageIndex = MaxPage;
    Search();
    return false;
}
//上一页
function onToPrev() {
    if (PageIndex - 1 > 0) {
        PageIndex = PageIndex - 1;
    } else {
        PageIndex = 1;
    }
    Search();
    return false;
}
//下一页
function onToNext() {
    if (PageIndex + 1 < MaxPage) {
        PageIndex = PageIndex + 1;
    } else {
        PageIndex = MaxPage;
    }
    Search();
    return false;
}


//新增人员
function Add() {
    window.showModalDialog("frm_UserSetDts.htm", null, "dialogWidth=1000px;dialogHeight=500px");
    Search();
}
//删除人员
function Delete() {
    var row = gridManager.getSelected();
    if (row) {
        $.ligerDialog.confirm('确定删除？', function (yes) {
            if (yes) {
                var code = row.User_Code;
                var arr = new Array();
                arr[0] = code;
                ReturnMethod("BLL_UserSet", "DelUser", arr, Address, "DeleteSucess");
                Search();
            }
        });
    }
    else {
        $.ligerDialog.warn('请选择要删除的人员！');
        return;
    }
}
function Close() {
    parent.layer.closeAll('iframe');
}
var CustomMethod = {
    DeleteSucess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('删除成功！');
            Search();
        }
        else {
            $.ligerDialog.warn('删除失败！');
        }
    }
}
