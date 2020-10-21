var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var StuffCode = GetQueryString("code");

$(function () {
    InitControl();
    Search();
});

function Search() {
    //导出数据成功后加载数据
    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID(); //获取选择项ID

    var arr = new Array();
    arr[0] = StuffCode;
    arr[1] = Tid.replace("tabitem", "");
    arr[2] = "";
    arr[3] = "";
    arr[4] = PageIndex;
    arr[5] = PageNum;

    if (Tid == 'tabitem1') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "ValidData");
    }
    else if (Tid == 'tabitem2') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "AlreadyUser");
    }
    else if (Tid == 'tabitem3') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "UnsubscribeUser");
    }
    else if (Tid == 'tabitem4') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "AlreadyCallData");
    }
    else if (Tid == 'tabitem5') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "LoopData");
    }
    else if (Tid == 'tabitem6') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBBlackData", "GetSSBBlackDataCount", arr, "BlackUser");
    }
    else if (Tid == 'tabitem7') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "KeyWordData");
    }
    else if (Tid == 'tabitem8') {
        arr[1] = 9;
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "HighKeyWordData");
    }
    else if (Tid == 'tabitem9') {
        arr[1] = 8;
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherData");
    }
    else if (Tid == 'tabitem10') { 
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherDataTuiDing");
    }
    else if (Tid == 'tabitem11') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherNoValidData");
    }
    else { 
        GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherNameCommonData");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //点击选项加载数据
    $("#navtab1").ligerTab({
        onAfterSelectTabItem: function (tabid) {
            PageIndex = 1;
            var arr = new Array();
            arr[0] = StuffCode;
            arr[1] = tabid.replace("tabitem", "'");
            arr[2] = "";
            arr[3] = "";
            arr[4] = PageIndex;
            arr[5] = PageNum;
            if (tabid == 'tabitem1') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "ValidData");
            }
            else if (tabid == 'tabitem2') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "AlreadyUser");
            }
            else if (tabid == 'tabitem3') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "UnsubscribeUser");
            }
            else if (tabid == 'tabitem4') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "AlreadyCallData");
            }
            else if (tabid == 'tabitem5') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "LoopData");
            }
            else if (tabid == 'tabitem6') {
                var arr = new Array();
                arr[0] = StuffCode;
                arr[1] = PageIndex;
                arr[2] = PageNum;
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBBlackData", "GetSSBBlackDataCount", arr, "BlackUser");
            }
            else if (tabid == 'tabitem7') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "KeyWordData");
            }
            else if (tabid == 'tabitem8') {
                arr[1] = 9;
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetViewDataCount", arr, "HighKeyWordData");
            }
            else if (tabid == 'tabitem9') {
                arr[1] = 8;
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherData");
            }
            else if (tabid == 'tabitem10') { 
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherDataTuiDing");
            }
            else if (tabid == 'tabitem11') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherNoValidData");
            }
            else { 
                GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "OtherNameCommonData");
            }
        }
    })
}

function InitControl() {
    //初始化工具条
    $("#toptoolbar").ligerToolBar({
        items: [{ text: '导出', icon: 'up', click: OutExcel }
        ]
    });

    //初始化选项
    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID(); //获取选择项ID

    if (Tid == 'tabitem1') {
        gridManager = $("#ValidData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ], pageSize: 20, width: '99%', height: '98%',
            pageSizeOptions: 20,
            enabledSort: false,
            usePager: true,
            onToFirst: onToFirst,
            onToPrev: onToPrev,
            onToNext: onToNext,
            onToLast: onToLast
        })
    }
    else if (Tid == 'tabitem2') {
        gridManager = $("#AlreadyUser").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ],
            pageSize: 20
        })
    }
    else if (Tid == 'tabitem3') {
        gridManager = $("#UnsubscribeUser").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ],
            pageSize: 20
        })
    }
    else if (Tid == 'tabitem4') {
        gridManager = $("#AlreadyCallData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ],
            pageSize: 20
        })
    }
    else if (Tid == 'tabitem5') {
        gridManager = $("#LoopData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ],
            pageSize: 20
        })
    }
    else if (Tid == 'tabitem6') {
        gridManager = $("#BlackUser").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                { display: '提供来源', name: 'RB_Provider', minWidth: 90 }

            ],
            pageSize: 20
        })
    }
    else if (Tid == 'tabitem7') {
        gridManager = $("#KeyWordData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ],
            pageSize: 20
        })
    }
    else if (Tid == 'tabitem8') {
        gridManager = $("#HighKeyWordData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
            ]
        })
    }
    else if (Tid == 'tabitem9') {
        gridManager = $("#OtherData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
            ]
        })
    }
    else if (Tid == 'tabitem10') {
        gridManager = $("#OtherDataTuiDing").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                { display: '新增时间', name: 'AddDate', minWidth: 90 },
                { display: '退订时间', name: 'TDDate', minWidth: 90 },
                { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
            ]
        })
    }
    else if (Tid == 'tabitem11') {
        gridManager = $("#OtherNoValidData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                { display: '新增时间', name: 'AddDate', minWidth: 90 },
                { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
            ]
        })
    }
    else {
        gridManager = $("#OtherNameCommonData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
            ]
        })
    }
    // GridInitIDControl("ValidData");
    GridInitIDControl("AlreadyUser");
    GridInitIDControl("UnsubscribeUser");
    GridInitIDControl("AlreadyCallData");
    GridInitIDControl("LoopData");
    GridInitIDControl("BlackUser");
    GridInitIDControl("KeyWordData");
    GridInitIDControl("HighKeyWordData");
    GridInitIDControl("OtherData");
    GridInitIDControl("OtherDataTuiDing");
    GridInitIDControl("OtherNoValidData");
    GridInitIDControl("OtherNameCommonData");

    ///////////////////////////////////////////////////////////////
    //初始化选项点击事件
    $("#navtab1").ligerTab({
        onAfterSelectTabItem: function (tabid) {
            if (tabid == 'tabitem1') {
                gridManager = $("#ValidData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ], pageSize: 20, width: '99%', height: '98%',
                    pageSizeOptions: 20,
                    enabledSort: false,
                    usePager: true,
                    onToFirst: onToFirst,
                    onToPrev: onToPrev,
                    onToNext: onToNext,
                    onToLast: onToLast
                })
            }
            else if (tabid == 'tabitem2') {
                gridManager = $("#AlreadyUser").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ],
                    pageSize: 20
                })
            }
            else if (tabid == 'tabitem3') {
                gridManager = $("#UnsubscribeUser").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ],
                    pageSize: 20
                })
            }
            else if (tabid == 'tabitem4') {
                gridManager = $("#AlreadyCallData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ],
                    pageSize: 20
                })
            }
            else if (tabid == 'tabitem5') {
                gridManager = $("#LoopData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ],
                    pageSize: 20
                })
            }
            else if (tabid == 'tabitem6') {
                gridManager = $("#BlackUser").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                        { display: '提供来源', name: 'RB_Provider', minWidth: 90 }
                    ],
                    pageSize: 20
                })
            }
            else if (tabid == 'tabitem7') {
                gridManager = $("#KeyWordData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ],
                    pageSize: 20
                })
            }
            else if (tabid == 'tabitem8') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#HighKeyWordData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 }
                    ]
                })
            }
            else if (tabid == 'tabitem9') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#OtherData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                        { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
                    ]
                })
            }
            else if (tabid == 'tabitem10') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#OtherDataTuiDing").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                        { display: '新增时间', name: 'AddDate', minWidth: 90 },
                        { display: '退订时间', name: 'TDDate', minWidth: 90 },
                        { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
                    ]
                })
            }
            else if (tabid == 'tabitem11') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#OtherNoValidData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                        { display: '新增时间', name: 'AddDate', minWidth: 90 },
                        { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
                    ]
                })
            }
            else {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#OtherNameCommonData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '座机号码', name: 'ODD_Phone', minWidth: 90 },
                        { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
                    ]
                })
            }
            
            GridInitIDControl("AlreadyUser");
            GridInitIDControl("UnsubscribeUser");
            GridInitIDControl("AlreadyCallData");
            GridInitIDControl("LoopData");
            GridInitIDControl("BlackUser");
            GridInitIDControl("KeyWordData");
            GridInitIDControl("HighKeyWordData");
            GridInitIDControl("OtherData");
            GridInitIDControl("OtherDataTuiDing");
            GridInitIDControl("OtherNoValidData");
            GridInitIDControl("OtherNameCommonData");
        }
    })
}

function OutExcel() {
    var arr = new Array();

    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID();
    arr[0] = StuffCode;
    arr[1] = "SSB_";
    if (Tid == 'tabitem1') {
        arr[1] = arr[1] + "ValidData";
    }
    else if (Tid == 'tabitem2') {
        arr[1] = arr[1] + "AlreadyUse";
    }
    else if (Tid == 'tabitem3') {
        arr[1] = arr[1] + "UnsubscribeUser";
    }
    else if (Tid == 'tabitem4') {
        arr[1] = arr[1] + "AlreadyCallData";
    }
    else if (Tid == 'tabitem5') {
        arr[1] = arr[1] + "LoopData";
    }
    else if (Tid == 'tabitem6') {
        arr[1] = arr[1] + "BlackUser";
    }
    else if (Tid == 'tabitem7') {
        arr[1] = arr[1] + "KeyWordData";
    }
    else if (Tid == 'tabitem8') {
        arr[1] = arr[1] + "HighKeyWordData";
    }
    else if (Tid == 'tabitem9') {
        arr[1] = arr[1] + "OtherData";
    }
    else if (Tid == 'tabitem10') {
        arr[1] = arr[1] + "OtherDataTuiDing";
    }
    else if (Tid == 'tabitem11') {
        arr[1] = arr[1] + "OtherNoValidData"; 
    }
    else {
        arr[1] = arr[1] + "OtherNameCommonData";
    }
    arr[2] = "";
    arr[3] = "";

    var data = ReturnValue("BLL_OrigDataImport", "DtToExcel", arr, Address);
    if (data == "true")
        $.ligerDialog.open({ url: "../../Aspx/ImportHelper.aspx" });
    else
        $.ligerDialog.error('当前内容为空,无法导出！');
}
