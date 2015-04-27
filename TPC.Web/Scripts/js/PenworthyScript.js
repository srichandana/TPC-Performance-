(function ($) {
    $(document).ready(function () {
        $('#txtSearch').keyup(function (e) {
            if (e.keyCode == 13) {
                var searchText = $(this).val();
                if (searchText != "") {
                    searchText = searchText.trim();
                    var searchUrl = $('#hdnSearchUrl').val() + '?groupID=0&currentPageIndex=1&noofItemsPerPage=60&quoteID=0&searchText=' + searchText;
                    window.location.href = searchUrl;
                }
            }
        });

        var hdnImagesPath = $('#hdnImagesPath').val();
        var Issingleview = false;
        $(".carouselRight").hover(function () {
            $(this).css("background-color", "#00a0cf");
            $(this).children('img').attr('src', hdnImagesPath + 'Right-Arrow-white.png');
        }, function () {
            $(this).css("background-color", "rgba(0, 0, 0, 0.06)");
            $(this).children('img').attr('src', hdnImagesPath + 'Right-Arrow.png');
        });
        $(".carouselLeft").hover(function () {
            $(this).css("background-color", "#00a0cf");
            $(this).children('img').attr('src', hdnImagesPath + 'Left-Arrow-white.png');
        }, function () {
            $(this).css("background-color", "rgba(0, 0, 0, 0.06)");
            $(this).children('img').attr('src', hdnImagesPath + 'Left-Arrow.png');
        });

        $("#loading-div-background").css({ opacity: 0.8 });

        var $loading = $('#loading-div-background').hide();

        var datetimenow = $('#hdnDateTime').val();
        var authenticationStatus = $('#hdnAuthenticationStatus').val();
        var custroleexist = $('#hdnCustRoleExists').val();
        var CustomerStatus = $('#hdnCustStatus').val();
        var qid;
        var itemId;
        var label;
        var dwName;
        var quoteTypeID = 0;
        var pageViewTitle = $('#hdnPageTitle').val();

        $('.NumericUpDownBtns').unbind('click');
        $('#ulBreadcrumb,#ulBreadcrumbCust').html('');
        $('.navlihide').hide();

        BreadCrumb();
        if ($("#hdnTitle").val() == "CatalogInformation") {
            CatalogingScript();
        }
        $(".confirmDialog").live("click", function () {
            label = this.id;
            qid = $(this).attr('data-quoteID');
            dwName = $(this).attr('data-dwName');
            quoteTypeID = $(this).attr('data-QuoteTypeID');
            itemId = this.title;
            var ItemTitle = $(this).attr('data-Title') == undefined ? "" : $(this).attr('data-Title');
            if (itemId == 'DeleteAll') {
                $('#lbldeleteID').html('Are you sure you want to <b>Delete all Items</b> ?');
            }
            else if (itemId == 'DeleteDW' || itemId == 'DeleteQuote') {
                if (itemId == 'DeleteDW') {
                    if (dwName == 'Penworthy') {
                        $("#h3Title").text("Empty Decision wizard");
                    }
                    else {
                        $("#h3Title").text("Delete Decision wizard");
                    }
                }
                else {
                    $("#h3Title").text("Delete Quote");
                }
                if (dwName == 'Penworthy') {
                    $('#lbldeleteID').html('Are you sure you want to <strong>Empty all Items</strong> ?');
                }
                else {
                    $('#lbldeleteID').html(' Are you sure you want to delete this ' + label + ' ?');
                }
            }
            else {
                $('#lbldeleteID').html('Are you sure you want to delete <strong>' + ItemTitle + '</strong> ?');
            }

        });

        var includeCatalog = false;

        if (pageViewTitle == "ActiveQuote") {
            ActiveQuoteScript();
        }

        if (pageViewTitle == "ShoppingCart") {
            ShoppingCartScript();
        }
        if (pageViewTitle == "Categories" || pageViewTitle == "Products" || pageViewTitle == "HomePage" || pageViewTitle == "Search") {
            CategoriesViewScript();
        }

        if (pageViewTitle == "QuoteView") {
            quoteViewScript();
        }

        if (pageViewTitle == "DecisionWizard") {
            TotalTitles();
            CalculatePrice();

        }
        if (pageViewTitle == "ItemListView") {
            itemListViewScript();
        }
        if (pageViewTitle == "HomePage" || pageViewTitle == "Log in") {
            indexPageScript();
            $(".markers").css("z-index", "0");
        }
        CommonScript();
        LibraraianResourcesScript();
        if (pageViewTitle == "KPL") {
            KPLViewBarcodeScript();
        }

        function QuoteLinksDisable(statusid) {
            if (statusid != undefined) {
                var datastatusid = $('#hdnstatusID').val();
                $(".disablekpl").removeAttr('disabled');
                $(".disablecategory").removeAttr('disabled');

                if ($(".disbalmerge").attr('href')) {
                }
                else {
                    $(".disbalmerge").css("color", "#2e92cf").attr('href', '#small-dialog');
                }
                if ($(".disableSubmit").attr('href')) {
                }
                else {
                    $(".disableSubmit").css("color", "#2e92cf").attr('href', '#SubmitQuote-dialog');
                }
                if ($(".disableRapidEntry").attr('href')) {
                }
                else {
                    $(".disableRapidEntry").css("color", "#2e92cf").attr('href', '#RapidEntry-dialog');
                }
                if (datastatusid == statusid) {
                    $(".disablekpl").attr('disabled', 'disabled');
                    $(".disablecategory").attr('disabled', 'disabled');
                    $(".disbalmerge").css("color", "#bebebe").removeAttr('href');
                    $(".disableSubmit").css("color", "#bebebe").removeAttr('href');
                    $(".disableRapidEntry").css("color", "#bebebe").removeAttr('href');

                }
            }

        }

        //Active Quote Script----------------------------------------------------------------------------------------Start
        function ActiveQuoteScript() {

            if ($('input.Quote').is(':checked')) {
            } else {
                $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr('checked', true);
                qid = $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr('data-quoteid');
                var id = $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr("data-statusid");
                QuoteLinksDisable(id);
                $('.tblDW').each(function () {
                    $(this).find('tbody').find('tr:first-child').find('td:nth-child(' + (1) + ')').find('#assignDWChkBx').attr('checked', true)//.each(function () {
                });
            }
            //active quote pop up
            //$(".btnSendInfo").click(function () {
            //    $("#CreateQuoteView").html($("#SendEmaildiv").html());

            //});



            //$("#mailQuote").live('click', function () {
            //    SendMail("#tblQuote");
            //});
            var isDWEmail;
            var isOrderhistory;
            var selectedDWlistID;
            $(".btnSendInfo").click(function () {
                $("#CreateQuote-dialog").removeAttr('style');
                $("#CreateQuote-dialog").css('height', '220px');
                isDWEmail = this.hasAttribute("data-mail");
                isOrderhistory = $(this).hasClass("Orderhistory");
                if (isDWEmail) {
                    $("#CreateQuote-dialog").removeAttr('style');
                    $("#CreateQuote-dialog").css('height', '240px');
                    selectedDWlistID = $(this).attr('id');
                    $("#CreateQuoteView").html($("#SendEmaildiv").html());
                    $("#CreateQuoteView").find("#pdfOrIntialText").text("Initial");
                    $("#CreateQuoteView").find("#IntialTextOnly").text("Initial with no images");
                    $("#CreateQuoteView").find("#excelOrRemainderText").text("Reminder");
                    $("#CreateQuoteView").find("#IntialNewUserText").text("Initial New User");
                    $("#CreateQuoteView").find('.sendDWOption').removeClass('hide');
                }
                else {
                    $("#CreateQuoteView").html($("#SendEmaildiv").html());
                    $("#CreateQuoteView").find("#pdfOrIntialText").text("Export Pdf");
                    $("#CreateQuoteView").find("#excelOrRemainderText").text("Export Excel");
                    $("#CreateQuoteView").find('.sendQuoteOption').removeClass('hide');
                    $("#CreateQuoteView").find("#tr_IntialTextOnly").hide();
                    $("#CreateQuoteView").find("#tr_InitalNewUser").hide();
                }

            });
            var selectedOption;
            $("#mailQuote").live('click', function () {
                var repComments = $('#txtRepCommnts').val().trim();
                if (isDWEmail) {
                    $('.sendDWOption').each(function () {
                        if ($(this).attr('checked') == 'checked') {
                            selectedOption = this.value;
                        }
                    });

                    var dwquoteIds = [];
                    var selectedindex = 0;//$('#UserVM_CRMModelProperties_LoggedINCustomerUserID option:selected').index();
                    $('#tblDW-' + selectedindex).find('input:checked').each(function () {
                        var quoteId = $(this).attr('data-quoteID');
                        dwquoteIds.push(quoteId);
                    });

                    $.magnificPopup.close();
                    if (dwquoteIds.length > 0) {
                        $.ajax({
                            url: "../ActiveQuote/UpdateRecentContactedByQuoteIDs",
                            type: "POST",
                            data: { quoteIDs: dwquoteIds.toString(), selectedTemplate: selectedOption, repComment: repComments },
                            datatype: 'html',
                            success: function (data) {
                                $('#tblDW-0').find('#dw-' + dwquoteIds.toString().trim() + ' td').each(function () {
                                    if ($(this).attr('id') == 'dwcreateDate') {
                                        if (data != '') {
                                            $(this).text(data);
                                        }
                                    }
                                });
                                var recentUpdatedEmailDate = $('#hdnDateTimeWithFormat').val();
                                $('#DwInitateDate').html(recentUpdatedEmailDate);
                            }
                        });
                    }
                }
                else {
                    if (isOrderhistory) {
                        SendMail("#InvoiceHistory");

                    }
                    else {
                        SendMail("#tblQuote");

                    }
                }
            });

            function SendMail(tableId) {
                var isPdf = $("#Pdf").is(':checked');
                var isExcel = $("#Excel").is(':checked');
                var quoteCheckboxID = tableId == "#tblQuote" ? "#assignChkBx" : "#HistoryChkBx";
                var qstr = tableId == "#InvoiceHistory" ? "&invoicePdf=true" : "";
                var repComments = $('#txtRepCommnts').val().trim();
                var mailtoaddress = '';
                var quoteid = "";
                $(tableId + " tbody tr").each(function () {
                    if ($(this).find(quoteCheckboxID).is(':checked')) {
                        var mailURL = $('#hdnMailURL').val();
                        var pdfmailURL = $('#hdnPdfDWURL').val();
                        quoteid = $(this).find(quoteCheckboxID).attr('data-quoteid');
                        var pdfFilename = $(this).find(quoteCheckboxID).attr('data-pdf');
                        var mailSubject = isOrderhistory ? $('#hdnInvoiceMailSubject').val() : $('#hdnMailSubject').val();
                        var urls = isExcel == true ? encodeURIComponent(mailURL + $(this).find(quoteCheckboxID).attr('data-quoteid') + "&isPdf=false&isExcel=" + isExcel) : "";
                        if (isPdf && tableId == "#InvoiceHistory") 
                            urls = encodeURIComponent(pdfmailURL + $(this).find(quoteCheckboxID).attr('data-filename'));
                        else 
                            urls = '%0D%0A' + encodeURIComponent(mailURL + $(this).find(quoteCheckboxID).attr('data-quoteid') + "&isPdf=" + isPdf + "&isExcel=false" + qstr);
                        //console.log(urls);
                        urls = repComments != undefined && repComments != "" ? repComments + '%0D%0A' + urls : urls;
                        urls = qstr == "" ? urls : pdfFilename == null || pdfFilename == "" ? repComments + '%0D%0A' + "no PDF exists" : urls;
                        $('#mailQuote').attr("href", "mailto: " + $('a[id=' + $("#hdnDefaultUID").val() + ']').attr('data-mail') + "?subject=" + mailSubject + " &body= " + urls);
                        mailtoaddress = $('a[id=' + $("#hdnDefaultUID").val() + ']').attr('data-mail');
                    }
                });
                $.ajax({
                    url: "../ActiveQuote/SaveData",
                    type: "POST",
                    data: { toAddress: mailtoaddress, quoteId: quoteid },
                    datatype: 'html',
                    success: function (data) {
                        if (tableId == "#tblQuote") {
                            var recentUpdatedEmailDate = $('#hdnDateTimeWithFormat').val();
                            $('#QuoteInitateDate').html(recentUpdatedEmailDate);
                        }
                    }
                });
                $.magnificPopup.close();
            }

            $('.CreateDW').on("click", function () {
                $("#CreateQuote-dialog").css('height', '150px');
                var selectid = this.id;
                $.ajax({
                    url: "../ActiveQuote/CreateDWPartial",
                    type: "POST",
                    datatype: 'html',
                    success: function (data) {
                        $('#CreateQuoteView').html('');
                        $('#CreateQuoteView').html(data);
                    }
                });
            });

            $('.CreateQuote').on("click", function () {
                $("#CreateQuote-dialog").css('height', '180px');
                var selectid = this.id;
                $.ajax({
                    url: "../ActiveQuote/CreateQuotePartial",
                    type: "POST",
                    datatype: 'html',
                    success: function (data) {
                        $('#CreateQuoteView').html('');
                        $('#CreateQuoteView').html(data);
                    }
                });
            });

            ////submitting the Quote-Opens PopUp

            $('.SubmitQuote').on("click", function () {
                //SubmitQuotePartialView
                SubmitQuoteFunctionality();
            });

            $('.RapidEntry').live("click", function () {
                RapidEntryFunctionality($(this));
            });


            var defaultCustomerID = $('#hdnDefaultCustomerID').val();
            $('#UserVM_CRMModelProperties_LoggedINCustomerUserID').on("change", function () {
                var index = $('option:selected', $(this)).index();
                var customerID = $('#UserVM_CRMModelProperties_LoggedINCustomerUserID').val();
                $('.CustomerInfo').addClass('hide');
                $('#CustomerInfo-' + customerID).removeClass('hide');
                $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr('checked', true)
                $('.tblDW').each(function () {
                    $(this).find('tbody').find('tr').find('td').find('#assignDWChkBx').attr('checked', false)
                });
                $('.tblDW').each(function () {
                    $(this).find('tbody').find('tr:first-child').find('td:nth-child(' + (1) + ')').find('#assignDWChkBx').attr('checked', true)
                });
            });

            //hiding the row of same quote Type selected for Merge
            $('.Merge').on("click", function () {

                $("#tblMergeQuote").html($("#tblQuote").html());
                $("#tblMergeQuote").find("thead tr").find('th:last').remove();
                $("#tblMergeQuote").find("tr").find('#quoteDialog').remove();
                $("#tblMergeQuote").find("thead").removeClass('nofixedHeader').addClass('noMfixedHeader');
                $("#tblMergeQuote").find("tbody").removeClass('noscrollContent').addClass('noMscrollContent');
                $('#small-dialog').css("width", "940px");
                $('#tblMergeQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                $('#tblMergeQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').prop('checked', true);
                $("#tblMergeQuote").find("tbody").find('tr').find('td:first-child').children('input[type=checkbox]').removeAttr('class');
                $('#MergeQuoteData').removeClass('hide');
                $('#DWMailData').addClass('hide');
                $('#MailButtons').addClass('hide');
                $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').prop('checked', true);
            });

            //Add Merge Quotes
            $('#mergebtn').live("click", function () {
                var quoteIdlist = [];
                var quoteTypeID;
                var totalItems = 0;
                var totalPrice = 0;
                $('#tblMergeQuote input:checked').each(function () {
                    var quoteId = $(this).attr('data-quoteID');
                    quoteIdlist.push(quoteId);
                    quoteTypeID = $(this).attr('data-quoteType');
                });
                $.ajax({
                    url: "../ActiveQuote/AddMergeQuotes",
                    type: "POST",
                    data: { quoteIDs: quoteIdlist.toString(), customerID: $('#hdncustid').val(), quoteType: quoteTypeID },
                    datatype: 'html',
                    success: function (data) {
                        var $trow = $("#tblQuote").find('tfoot').find('tr').clone();
                        if ($('#tblQuote').find('tbody').find('tr').length >= 0) {
                            $('#tblQuote').find('thead').removeClass('hide');
                            $(".quoteLinks").removeAttr('disabled');
                            $("#mergeQuote").css("color", "#2e92cf").attr('href', '#small-dialog');
                            $("#submitQuoteInfo").css("color", "#2e92cf").attr('href', '#SubmitQuote-dialog');
                            $(".disableRapidEntry").css("color", "#2e92cf").attr('href', '#RapidEntry-dialog');
                        }
                        $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                        $trow.find('td').each(function (i) {
                            if (this.id == "chkQuoteInfo") {
                                $(this).children('input').attr('data-quoteid', data["QuoteID"]);
                                $(this).children('input').attr('data-quotetype', data["QuoteTypeID"]);
                                $(this).children('input').attr('value', (data["QuoteID"] + "%" + data["QuoteTypeText"] + "%" + data["TotalItems"] + "%" + data["TotalPrice"]));
                                $(this).children('input').attr('checked', 'checked');
                            }
                            else if (this.id == "quoteCreateDate") {
                                $(this).text(datetimenow.toString('g'));
                                //var tempDate = new Date(parseInt(data.CreatedDate.substr(6)));
                                //var currentdate = (tempDate.getMonth() + 1) + "/" + tempDate.getDate() + "/" + tempDate.getFullYear() + " " + (tempDate.getHours() > 12 ? tempDate.getHours() - 12 : tempDate.getHours()) + ":" + (tempDate.getMinutes() < 9 ? "0" + tempDate.getMinutes() : tempDate.getMinutes()) + " " + (tempDate.getHours() >= 12 ? "PM" : "AM");
                                //$(this).text(currentdate.toString());
                            }
                            else if (this.id == "quoteUpdateDate") {
                                //var tempDate = new Date(parseInt(data.UpdateDate.substr(6)));
                                //var updatedate = (tempDate.getMonth() + 1) + "/" + tempDate.getDate() + "/" + tempDate.getFullYear() + " " + (tempDate.getHours() > 12 ? tempDate.getHours() - 12 : tempDate.getHours()) + ":" + (tempDate.getMinutes() < 9 ? "0" + tempDate.getMinutes() : tempDate.getMinutes()) + " " + (tempDate.getHours() >= 12 ? "PM" : "AM");
                                //$(this).text(updatedate.toString());
                                $(this).text(datetimenow.toString('g'));
                            }
                            else if (this.id == "quoteName") {
                                $(this).text(data["QuoteName"]);
                            }
                            else if (this.id == "quotestatus") {
                                $(this).text(data["StatusText"]);
                            }
                            else if (this.id == "quoteTitle") {
                                $(this).text((data["QuoteTypeText"]));
                            }
                            else if (this.id == "quoteDialog") {
                                $(this).children('a').attr('data-quoteid', data["QuoteID"]);
                            }
                            else if (this.id == "quoteItems") {
                                $(this).text(data["TotalItems"]);
                            }
                            else if (this.id == "quotePrice") {
                                var totalPrice = parseFloat(data["TotalPrice"]).toFixed(0);
                                $(this).text("$" + totalPrice);
                            }
                        });
                        var trow1 = '<tr id="Quote-' + data["QuoteID"] + '">' + $trow.html() + '</tr>';
                        $("#tblQuote tbody:last").append(trow1);
                        $('.popup-with-zoom-anim').magnificPopup({
                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                        $('.tablesorter').trigger('update');
                        $(".quoteLinks").removeAttr('disabled');
                    }

                });
                $.magnificPopup.close();
            });

            $(".Quote").live("click", function () {
                $(".Quote").each(function () {
                    $(this)[0].checked = false;
                });
                $(this)[0].checked = true;
                qid = $(this).attr('data-quoteid');
            });

            $(".OrderHistory").live("click", function () {
                $(".OrderHistory").each(function () {
                    $(this)[0].checked = false;
                });
                $(this)[0].checked = true;
            });

            // for Dw checkbox
            $(".DWQuoteID").live("click", function () {
                $(".DWQuoteID").each(function () {
                    $(this)[0].checked = false;
                });
                $(this)[0].checked = true;
            });

            $("#assignChkBx").live("click", function () {
                var statusid = $(this).attr("data-statusid");
                QuoteLinksDisable(statusid)
            });



            $('#btnDeleteQuote').unbind().on("click", function () {
                if (dwName == "Penworthy") {
                    $.ajax({
                        url: "../ShoppingCart/DeleteItem",
                        type: "POST",
                        data: { quoteID: qid, item: 'DeleteAll', QuoteTypeID: quoteTypeID },
                        datatype: 'html',
                        success: function (data) {
                            $('#dw-' + qid + ' > #dwNBooks ,#dw-' + qid + ' > #dwYNCount ,#dw-' + qid + ' > #dwNCount ,#dw-' + qid + ' > #dwMCount ,#dw-' + qid + ' > #dwNWCount ').html('0');
                            $('#dw-' + qid + ' > #dwTPrice').html('$0');
                            $('#tblDW-0').find('tbody').find('tr[id=dw-' + qid + ']').find('td[id=dwcreateDate]').text(datetimenow.toString('g'));
                        }
                    });
                }
                else {
                    if (label == "DW") { $('tr[id=dw-' + qid + ']').hide(); }
                    else { $('tr[id=Quote-' + qid + ']').hide(); }
                    $.ajax({
                        url: "../ActiveQuote/DeleteQuote",
                        type: "POST",
                        data: { quoteID: qid },
                        datatype: 'html',
                        success: function (data) {
                            if (label == "DW") {
                                $('tr[id=dw-' + qid + ']').remove();
                                $(".tblDW").each(function (j) {
                                    $(this).find('tbody').find('tr').find('td').find('#assignDWChkBx').attr('checked', false);
                                    var tblcount = $(this).find(' tbody tr').length;
                                    $(this).find('tbody').find('tr:first-child').find('td:nth-child(' + (1) + ')').find('#assignDWChkBx').attr('checked', true);
                                });
                            }
                            else {
                                if ($('#tblQuote').find('tbody').find('tr').length == 1) {
                                    $(".quoteLinks").attr('disabled', 'disabled');
                                    $("#quoteEmailLink").removeAttr('href');
                                    $("#mergeQuote").css("color", "#bebebe").removeAttr('href');
                                    $("#submitQuoteInfo").css("color", "#bebebe").removeAttr('href');
                                    $('#submitRapidEntry').css("color", "#bebebe").removeAttr('href');
                                    $('#tblQuote').find('thead').addClass('hide');
                                    $('#QuoteInitateDate').html('');
                                }
                                $('tr[id=Quote-' + qid + ']').remove();
                                if ($('#tblQuote').find('tbody').find('tr').length >= 1) {
                                    var id = $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr("data-statusid");
                                    QuoteLinksDisable(id);
                                }
                                $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                                $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr('checked', true);
                            }
                            $('.tablesorter').trigger('update');
                        },
                        error: function (data) {
                            if (label == "DW") { $('tr[id=dw-' + qid + ']').show(); }
                            else { $('tr[id=Quote-' + qid + ']').show(); }
                        }
                    });
                }
                $.magnificPopup.close();
            });



            //DW -Mail
            var mail;
            //$('.sendEmail').on("click", function () {
            //    $('#DWMailData').removeClass('hide');
            //    $('#MergeQuoteData').addClass('hide');
            //    $('#MailButtons').removeClass('hide');
            //    var selectedDWlistID = $(this).attr('id');
            //    mail = $(this).attr('data-mail');
            //    $('.divDW').each(function () {
            //        if ($(this).attr('id') == selectedDWlistID) {
            //            $('#DWMailData').html($(this).html());
            //            //$("#DWMailData").find("thead").removeClass('nofixedHeaderDW').addClass('nofixedHeader');
            //            $("#DWMailData").find("tbody").removeClass('noscrollContentDW').addClass('noscrollMContentDW');
            //            $('#small-dialog').css("height", "630px");
            //            $('#small-dialog').css("width", "980px");
            //            $('#DWMailData').find('table').attr('id', 'tblMailDW').removeClass('tablesorter');
            //            $('#DWMailData').find('table').find('tbody').addClass('');
            //            $('#DWMailData').find('table').find('th:nth-child(' + (11) + '),td:nth-child(' + (11) + ')').addClass('hide');
            //        }
            //    });
            //    $('#DWMailData').find('table').find('tbody').find('tr').find('td:first-child').children('input[type=checkbox]').removeAttr('class');
            //});

            $('#mailokbtn').on("click", function () {
                var dwquoteIds = [];
                //var bodyMailText = 'Please click on the below link to see %0D%0A';
                //var mailURL = $('#hdnMailURL').val();
                //var mailSubject = $('#hdnMailSubject').val();
                $('#tblMailDW').find('input:checked').each(function () {

                    var quoteId = $(this).attr('data-quoteID');

                    //  var quoteTypeText = $(this).attr('data-QuoteTypeText');
                    dwquoteIds.push(quoteId);
                    //bodyMailText = bodyMailText + quoteTypeText + ' Decision Wizard : ' + mailURL + quoteId + '%0D%0A';
                    //$('#mailokbtn').attr("href", "mailto: " + mail + "?subject=" + mailSubject + " &body=" + bodyMailText);
                });
                $.magnificPopup.close();
                if (dwquoteIds.length > 0) {
                    //$.ajax({
                    //    url: "../ActiveQuote/UpdateRecentContactedByQuoteIDs",
                    //    type: "POST",
                    //    data: { quoteIDs: dwquoteIds.toString() },
                    //    datatype: 'html',
                    //    success: function (data) {
                    //    }
                    //});
                }
            });



            $("#tblQuote").tablesorter();
            $("[id^=tblDW]").tablesorter();

            $("#accordion").accordion({
                animate: false,
            });
            $('.calTag').click(function () {
                var quoteId = $(this).attr("data-quoteID");
                $.ajax({
                    url: "../ActiveQuote/CalTagPartialView",
                    type: "POST",
                    data: { quoteId: quoteId },
                    datatype: 'html',
                    success: function (data) {
                        $('#loadingCalTageView').html('');
                        $('#loadingCalTageView').html(data);
                    },
                    error: function (e) {
                        alert(e.error);
                    }
                });
                $.magnificPopup.close();
            });
            if ($("#tblQuote").find("tbody tr").length == 0) {
                $(".quoteLinks").attr('disabled', 'disabled');//.css("pointer", "none"); //.addClass("metro input[type='button']").attr('disabled', 'disabled');
                $("#mergeQuote").css("color", "#bebebe").removeAttr('href');
                $("#quoteEmailLink").removeAttr('href');
                $("#submitQuoteInfo").css("color", "#bebebe").removeAttr('href');
                $(".disableRapidEntry").css("color", "#bebebe").removeAttr('href');
            }
            $(".dwlinks").each(function (i) {
                if ($(this).find('.divDW') == false) {
                    $('.dwviewlinks-' + i).addClass("metro input[type='button']").attr('disabled', 'disabled');
                }
            });

            if ($("#InvoiceHistory").find("tbody tr").length > 0) {
                $("#InvoiceHistory").find('tbody').find('tr:first-child').find('td:nth-child(' + (1) + ')').find('#HistoryChkBx').attr('checked', true);
            }
        }
        //Active Quote Script----------------------------------------------------------------------------------------End

        //Shopping Cart Script-------------------------------------------------------------------------------------Start
        function ShoppingCartScript() {

            $("#grid").tablesorter({
                headers: {
                    // assign the secound column (we start counting zero) 
                    6: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    7: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    8: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                    9: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    },
                }
            });

            var qid = $('#hdnQuoteID').val();
            var quoteTypeID = $('#hdnQuoteTypeID').val();
            var hdnIncludeCatalogStatus = $('#hdnIncludeCatalogStatus').val();
            if (hdnIncludeCatalogStatus == 'True') {
                $('.includeCatalog ').attr('checked', 'checked')
                includeCatalog = true;
            }
            $('#grid tr').each(function () {
                if ($(this).attr('id') == "-Catalog") {
                    var catalogStatus = $(this).attr('data-catalog');
                    $(this).children('th[id=Quantity]').children('div').find('div[id=Quantity]').find('input[type=text][id=Catalog]').attr('data-Catalog', catalogStatus);
                }
                if ($(this).attr('id') == "-Catalog" && hdnIncludeCatalogStatus == 'False') {
                    $(this).addClass('hide');
                }
            });

            TotalBooks();
            CalculatePrice();
            TotalTitles();

            //$('#btnOrder').on("click", function () {
            //    var poText = $('#PoText').val();
            //    var comments = $('#Comments').val();
            //    $.ajax({
            //        url: "../ShoppingCart/PlaceOrder",
            //        type: "POST",
            //        data: { quoteID: qid, POText: poText, comments: comments },
            //        datatype: 'html',
            //        success: function (data) {
            //            $('.gridscrollContent tr').remove();
            //            $('.gridfixedFooter  tr[id=-Catalog]').remove();
            //            $('#lblTotalTitles').html('0');
            //            $('#totalPrice').html('$0.00');
            //            $('#lblTotalPrice').html('$0.00');
            //            $('#lblTotalBooks').html('0');
            //            $('#SCItemCount sup').html('0');
            //            $('#SCItemPrice a').html('$0.00');
            //            $('.tablesorter').trigger('update');
            //            $("#btnOrder").attr('disabled', 'disabled');
            //            $('#SalesTax').html('$0.00');
            //            $('#PoText').val('');
            //            $('#Comments').val('');
            //            $('#countprice').html(0 + ' items ' + $('#totalPrice').html());
            //            //$.magnificPopup.close();
            //            var orderQuoteId = data;
            //            var url = '/ShoppingCart/CartDWPdfGeneration?cartDWID=' + orderQuoteId;
            //            $('#order-pdf').html('Loading Order PDF...');
            //            $('#order-pdf').html('<embed id="PDF" width="820px" height="700px" src="' + url + '" type="application/pdf" style="margin-left:26%" />').css('margin', '0 auto 0');

            //            $.magnificPopup.open({
            //                items: {
            //                    src: $('#order-pdfdialog').html(),
            //                    type: 'inline'
            //                },
            //                modal: true
            //            });

            //        }
            //    });
            //});
            $('#PoText').keypress(function () {
                $(this).css('border-color', '');
            });

            //$('.NumericUpDownTxt').keypress(function (event) {
            //    var qty = this.value;
            //    if (event.which >= 48 && event.which <= 57 && qty >= 1 && qty < 101) {
            //    }
            //    else {
            //        this.value = '';
            //        alert('Quantity Should be a number between 1 to 100');
            //    }
            //    $.ajax({
            //        success: function (data) {
            //            var result = $('<table />').append(data).find('#grid').html();
            //            $('#grid').html(result);
            //        }
            //    });
            //});

            $('#btnDeleteCart').live("click", function () {
                var quantity = $('#' + itemId).val();
                var levelType = [];
                var $trrow;
                var productLinepro = '';
                $.ajax({
                    url: "../ShoppingCart/DeleteItem",
                    type: "POST",
                    data: { quoteID: qid, item: itemId, QuoteTypeID: quoteTypeID },
                    datatype: 'html',
                    success: function (data) {
                        var scCount = $('#SCItemCount sup').html();
                        if (itemId == 'DeleteAll') {
                            $('.gridscrollContent tr').remove();
                            $('#lblTotalTitles').html('0');
                            $('#totalPrice').html('$0.00');
                            $('#lblTotalPrice').html('$0.00');
                            $('#lblTotalBooks').html('0');
                            $('#SCItemCount sup').html('0');
                            $('.tablesorter').trigger('update');
                            $('#SalesTax').html('$0.00');
                            $('#SCItemPrice a').html('$0.00');
                        }
                        else {
                            $trrow = $('tr[id=-' + itemId + ']');
                            productLinepro = $('tr[id=-' + itemId + ']').find('input[type=text][id=' + itemId + ']').attr('data-productline');
                            levelType = GetLevelTYpe($trrow);
                            $('tr[id=-' + itemId + ']').remove();
                            scCount = parseInt(scCount) - parseInt(quantity);
                            $('.tablesorter').trigger('update');
                            CalculatePrice();
                        }
                        TotalBooks();


                        TotalTitles();
                        if ($("#lblTotalTitles").text() == "0") {
                            $('.gridscrollContent tr').remove();
                            $('#lblTotalPrice').html('$0.00');
                            $('#totalPrice').html('$0.00');
                            $("#DeleteAllItems").removeAttr('href');
                            $("#btnOrder").attr('disabled', 'disabled');
                            $("#btnOrder").addClass('div-disable');
                            $('#SalesTax').html('$0.00');
                            $('#countprice').html('0 items $0.00');
                        }
                        if ($('.gridscrollContent tr').length > 0) {
                            //CalculatePrice();
                            UpdateCatalogQuantityItemPrice(-(quantity), '', '#grid', '', levelType, productLinepro);
                            CalculatePrice();
                        }
                        else {
                            $("#grid tr[id='-Catalog']").addClass("hide");
                        }
                    }
                });
                $.magnificPopup.close();
            });


        }




        function UpdateCatalogQuantityItemPrice(quantity, type, tableid, IncDecType, LevelType, ProductLineforPUPPRO) {
            $('input[type=text][id=Catalog]').each(function () {
                var catalogval = $(this).attr('data-Catalog');
                if (catalogval == "True") {
                    var itemTitle = $(this).parent('div').attr('data-title');
                    var currentquantity = parseInt($(this).attr('value'));
                    var leveltype = $(this).parent('div').attr('data-LevelType');
                    var itemid = $(this).parent('div').attr('data-itemid');
                    if (quantity == 0 && itemTitle != "Additional Protectors") {
                        currentquantity = 0;
                    }
                    var productline = $(this).parent('div').attr('data-productline');
                    var updatedQuantity = currentquantity;
                    if (type == "All") {
                        if (ProductLineforPUPPRO != 'PRO' && ProductLineforPUPPRO != 'PUP') {
                            if (productline == "CATT" || (leveltype == "AR" || leveltype == "RC" || leveltype == "Lexile")) {

                                if ($.inArray(leveltype, LevelType) > -1 && IncDecType == '') {
                                    quantity = quantity > 0 ? 1 : -1;
                                    updatedQuantity = (parseInt(currentquantity) + parseInt(quantity));

                                }
                                else if (IncDecType == '') {

                                    updatedQuantity = productline == "CATT" ? parseInt(currentquantity) == 1 && quantity == -1 ? 1 : quantity > 0 ? (parseInt(currentquantity) + parseInt(1)) : (parseInt(currentquantity) - parseInt(1)) : currentquantity;
                                }
                            }
                            else {

                                updatedQuantity = itemTitle != "Additional Protectors" ? $('#lblTotalBooks').html() : currentquantity;
                            }
                        }
                    }
                    else {
                        if (ProductLineforPUPPRO != 'PRO' && ProductLineforPUPPRO != 'PUP') {
                            if (productline == "CATT" || (leveltype == "AR" || leveltype == "RC" || leveltype == "Lexile")) {

                                if ($.inArray(leveltype, LevelType) > -1 && IncDecType == '') {
                                    var LevelQuantity = quantity > 0 ? 1 : -1;
                                    updatedQuantity = (parseInt(currentquantity) + parseInt(LevelQuantity));
                                }
                                else if (IncDecType == '') {

                                    // updatedQuantity = productline == "CATT" ? parseInt(currentquantity) == 1 && quantity == -1 ? 1 : quantity > 0 ? (parseInt(currentquantity) + parseInt(1)) : (parseInt(currentquantity) - parseInt(1)) : currentquantity;
                                    updatedQuantity = productline == "CATT" ? quantity > 0 ? (parseInt(currentquantity) + parseInt(1)) : (parseInt(currentquantity) - parseInt(1)) : currentquantity;
                                }

                            }
                            else {

                                //  updatedQuantity = itemTitle != "Additional Protectors" ? parseInt(currentquantity) == 1 && quantity == -1 ? 1 : (parseInt(currentquantity) + parseInt(quantity)) : currentquantity;
                                updatedQuantity = itemTitle != "Additional Protectors" ? (parseInt(currentquantity) + parseInt(quantity)) : currentquantity;
                            }
                        }
                    }
                    // var updatedQuantity = (type == "All" && itemTitle != "Additional Protectors") ? productline == "CATT" ? currentquantity : leveltype == "AR" || leveltype == "RC" || leveltype == "Lexile" ? currentquantity : $('#lblTotalBooks').html() : parseInt(currentquantity) == 1 && quantity == -1 ? 1 : itemTitle != "Additional Protectors" ? productline == "CATT" || leveltype == "AR" || leveltype == "RC" || leveltype == "Lexile" ? currentquantity : (parseInt(currentquantity) + parseInt(quantity)) : currentquantity;
                    $(this).val(updatedQuantity);
                    $(this).attr('value', updatedQuantity);
                    $(this).attr('data-currentvalue', updatedQuantity);
                }
            });
            $(tableid + ' tfoot tr').each(function () {
                if ($(this).attr('id') == "-Catalog") {
                    var itemquantity = $(this).find('th[id=Quantity]').children('div').children('div').find('input').attr('value');
                    if (itemquantity == 0) {
                        $(this).addClass('hide');
                    } else {
                        if ($('#Include_Cataloging').prop('checked') == false) {
                            $(this).addClass('hide');
                        }
                        else {
                            $(this).removeClass('hide');
                        }

                    }
                    var itemunitprice = $(this).find('th[id=ItemPrice]').html().replace('$', '').replace(',', '');
                    var totalItemPrice = parseInt(itemquantity) * parseFloat(itemunitprice);
                    $(this).find('th[id=TtlPrice]').children('div').html('$' + totalItemPrice.toFixed(2));
                }
            });
        }


        function TotalBooks() {
            var totalbooks = 0;
            $(".Quantity").each(function (i) {
                var trthId = $(this).parent('div').parent('div').parent('td').parent('tr').attr('id');
                var trId = $(this).parent('div').parent('div').parent('th').parent('tr').attr('id');
                if (trId != "-Catalog" && trthId != "trforisbn") {
                    totalbooks = parseInt($(this).attr('value')) + parseInt(totalbooks);
                }
            });
            $('#lblTotalBooks').html(totalbooks);
            $('#SCItemCount sup').html(totalbooks);
        }


        function CalculatePrice() {
            var totalPrice = 0;
            $('.calculatePrice').each(function () {
                if ($(this).parent('th').parent('tr').attr('id') != "-Catalog" && includeCatalog == false) {
                    var itemPrice = $(this).html().replace('$', '').replace(',', '');
                    totalPrice = parseFloat(itemPrice) + parseFloat(totalPrice);
                }
                else if (includeCatalog == true) {
                    var itemPrice = $(this).html().replace('$', '').replace(',', '');
                    totalPrice = parseFloat(itemPrice) + parseFloat(totalPrice);
                }
            });
            var taxPrice = 0;
            if ($('#SalesTax').attr('data-tx') != "0") {
                taxPrice = (totalPrice * parseFloat($('#SalesTax').attr('data-tx')));
            }
            if (pageViewTitle == "QuoteView") {
                var countprice = $('#QuoteViewgrid tbody tr').size() + ' items ' + '$' + (taxPrice + totalPrice).toFixed(2);
                $('#countprice').html(countprice);
            }
            else if (pageViewTitle == "ShoppingCart") {
                var countprice = $('#grid tbody tr').size() + ' items ' + '$' + (taxPrice + totalPrice).toFixed(2);
                $('#countprice').html(countprice);
            }
            $('#SalesTax').html('$' + taxPrice.toFixed(2));
            $('#totalPrice').html('$' + (taxPrice + totalPrice).toFixed(2));
            $('#lblTotalPrice').html('$' + (taxPrice + totalPrice).toFixed(2));
            $('#SCItemPrice a').html('$' + (taxPrice + totalPrice).toFixed(2));
        }

        function TotalTitles() {

            rows = 0;
            $('.gridscrollContent tr,.gridFourBodyContent tr').each(function () {
                if ($(this).attr('id') != "-Catalog") {
                    rows++;
                }
                //else if (includeCatalog == true) {
                //    rows++;
                //}
            });
            $('#lblTotalTitles').html(rows);
        }

        //Shooping Cart Script-----------------------------------------------------------------------------------------End
        //Cataloging Script----------------------------------------------------------------------------------------Start
        function CatalogingScript() {
            var emailRegex = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            var currncyValidation = /^\d{0,4}(\.\d{0,2})?$/;
            var numberValidation = /^\+?[0-9]+$/;
            var isValid = true;

            calculatecatalogPricePerBook();
            $("#tblCatalogInfo").find('tr').each(function () {
                if ($(this).find('td:first-child input[type=checkbox]').is(':checked')) {
                    $(this).find("input[type=checkbox]:gt(0)").removeProp("disabled");
                }
            });
            ValidationCatalogProfile();

            $('.chkStatus').click(function () {
                var trow = $(this).parent().parent();
                if (trow.find("input[type=checkbox]:first").is(":checked")) {
                    trow.find("input[type=checkbox]:gt(0)").removeAttr("disabled");
                    if ($("#chkShelfReady").attr('checked') == 'checked') {
                        $(trow).find('td:nth-child(4)').find('input[type=checkbox]').attr("checked", "checked");
                        $(trow).find('td:nth-child(4)').find('input[type=checkbox]').attr('style', 'pointer-events:none');
                    }
                }
                else {
                    trow.find('td:nth-child(4)').find('input[type=checkbox]').removeAttr('style');
                    trow.find("input[type=checkbox]:gt(0)").attr("disabled", "disabled");
                    trow.find("input[type=checkbox]:gt(0)").removeAttr("checked");
                }
                ValidationCatalogProfile();
            });


            $(".Subject").live("click", function () {
                if ($('input[type=checkbox][name=52]').is(':checked')) {
                    $(".Subject").each(function () {
                        $(this)[0].checked = false;
                    });
                    $(this)[0].checked = true;
                }
            });

            //Catalog -Checkbox Event
            $('.catalogPrice').live('click', function () {
                var trow = $(this).parent().parent();
                var isItemChecked = false;
                if ($(this).hasClass('Item')) {
                    isItemChecked = true;
                }
                calculatepricebyRow(trow, isItemChecked, false);
                ValidationCatalogProfile();
            });

            $("#chkShelfReady").live('click', function () {
                var status = $("#chkShelfReady").is(':checked');
                var chkItemStatus = false;
                var trow;
                $("#tblCatalogInfo").find('tbody tr').each(function () {
                    chkItemStatus = $(this).find('td:first-child input[type=checkbox]').is(':checked');
                    trow = $(this);
                    if (chkItemStatus) {
                        if (status) {
                            if ($(this).find('td:nth-child(4)').find('input[type=checkbox]').length > 0) {
                                $(this).find('td:nth-child(4)').find('input[type=checkbox]').attr('checked', 'checked');
                            }
                        }
                        else {
                            $(this).find('td:nth-child(4)').find('input[type=checkbox]').removeAttr('checked');
                        }
                    }
                    calculatepricebyRow(trow, false, true);
                });
            });

            function calculatepricebyRow(trow, isitemChecked, isShelfReadyAllChecked) {
                var chkItemStatus = false;
                var catalogItemId = 0;
                var protectorColumnIndex = 2;
                var shelfReadyColumnIndex = 3;
                var protectorId = 0, shelfReadyId = 0;
                var noofProtectorsChecked = 1;

                var noofProtectorsCount = $(trow).find('td').eq(2).find('input[type=checkbox]').length;
                chkItemStatus = $(trow).find('td:first-child').find('input[type=checkbox]').is(':checked');
                var shelfReadyAllstatus = $("#chkShelfReady").is(':checked');
                if (isitemChecked) {
                    if (chkItemStatus) {
                        $(trow).find('td:nth-child(' + (shelfReadyColumnIndex + 1) + ')').find('input[type=checkbox]').removeAttr('disabled');
                        $(trow).find('td:nth-child(' + (protectorColumnIndex + 1) + ')').find('input[type=checkbox]').removeAttr('disabled');
                        if (shelfReadyAllstatus == true) {
                            $(trow).find('td:nth-child(' + (shelfReadyColumnIndex + 1) + ')').find('input[type=checkbox]').attr('checked', 'checked');
                        }
                    }
                    else {
                        $(trow).find('td:nth-child(' + (protectorColumnIndex + 1) + ')').find('input[type=checkbox]').prop('checked', false);
                        $(trow).find('td:nth-child(' + (shelfReadyColumnIndex + 1) + ')').find('input[type=checkbox]').prop('checked', false);
                        $(trow).find('td:nth-child(' + (shelfReadyColumnIndex + 1) + ')').find('input[type=checkbox]').attr('disabled', "disabled");
                        $(trow).find('td:nth-child(' + (protectorColumnIndex + 1) + ')').find('input[type=checkbox]').attr('disabled', "disabled");
                    }
                }

                var protectorColumnChecked = $(trow).find('td:nth-child(' + (protectorColumnIndex + 1) + ')').find('input[type=checkbox]').is(':checked');
                var ShelfReadyColumnChecked = $(trow).find('td:nth-child(' + (shelfReadyColumnIndex + 1) + ')').find('input[type=checkbox]').is(':checked');

                for (var i = 1; i < noofProtectorsCount; i++) {
                    if ($(trow).find('td:nth-child(' + (protectorColumnIndex + 1) + ')').find('input[type=checkbox]')[i].checked) {
                        noofProtectorsChecked += 1;
                    }
                }

                if (chkItemStatus) {
                    catalogItemId = $(trow).find('td:first-child').find('input[type=checkbox]').attr('name');
                }

                if (protectorColumnChecked) {
                    protectorId = $(trow).find('td:nth-child(' + (protectorColumnIndex + 1) + ')').find('input[type=checkbox]').attr('name').split('-')[1];
                }

                if (ShelfReadyColumnChecked) {
                    shelfReadyId = $(trow).find('td:nth-child(' + (shelfReadyColumnIndex + 1) + ')').find('input[type=checkbox]').attr('name').split('-')[1];
                }
                if (chkItemStatus) {
                    $.ajax({
                        url: "../CatalogInfo/getCalculatedPriceBySubOptionIDs",
                        type: "POST",
                        async: false,
                        data: { catalogSubjOptionID: catalogItemId, protectorSubOptionID: protectorId, shelfReadySubOptionID: shelfReadyId, protectorsCheckedCount: noofProtectorsChecked },
                        datatype: 'html',
                        success: function (data) {
                            $(trow).find('td:last-child').text('').append(data);
                            calculatecatalogPricePerBook();
                        }
                    });
                }
                else {
                    var price = '$0.00';
                    $(trow).find('td:last-child').text('').append(price);
                    calculatecatalogPricePerBook();
                }
            }

            function calculatecatalogPricePerBook() {
                var totalprice = 0;
                $(".itmperprice").each(function () {
                    totalprice = totalprice + parseFloat($(this).text().replace('$', '').replace(',', ''));
                });
                $("#ttlprice").text('$' + totalprice.toFixed(2));
            }
            var selecteddl;
            //ddl for Other required commments
            $('.ddlShelf').change(function () {
                selecteddl = $(this);
                //if ($(this).find('option:selected').text() == 'A' || $(this).find('option:selected').text() == 'B' || $(this).find('option:selected').text() == 'C' || $(this).find('option:selected').text() == 'D') {
                //    $.magnificPopup.open({
                //        items: {
                //            src: $('#Delete-dialog').html(),
                //            type: 'inline'
                //        },
                //        modal: true
                //    });
                //}

                var otherSelected = false;
                if ($(this).attr('name') == "73") {
                    if ($(this).find('option:selected').text() == 'Other') {
                        otherSelected = true;
                    }
                    if (otherSelected) {
                        $("#bottomLabel").removeAttr('disabled');
                    }
                    else {
                        $("#bottomLabel").attr('disabled', 'disabled');
                    }
                }
            });

            $('#yes').live("click", function () {
                $.magnificPopup.close();
            });
            $('#no').live("click", function () {
                $(selecteddl).find('option:selected').removeAttr('selected');
                $(selecteddl).find('option[value=0]').attr('selected', 'selected');
                $.magnificPopup.close();
            });


            $(".catlogDDL").change(function () {
                var emailEnabled = false;
                var truncateEnabled = false;
                var otherSelected = false;
                if ($(this).attr('data-text') == "Media Type") {
                    if ($(this).find('option:selected').text() == 'Email' || $(this).find('option:selected').text() == 'Download') {
                        emailEnabled = true;
                    }
                    if (emailEnabled) {
                        $('label[data-id = 69]').parent().find('.starDisplay').removeClass('hide');
                    }
                    else {
                        $('label[data-id = 69]').parent().find('.starDisplay').addClass('hide');
                    }
                }
                else if ($(this).attr('data-text') == "Non-Fiction Dewey") {
                    if ($(this).find('option:selected').text() == 'Truncated - Number of Spaces') {
                        truncateEnabled = true;
                    }
                    if (truncateEnabled) {
                        $('input[type=text][name=76]').removeAttr('disabled');
                    }
                    else {
                        $('input[type=text][name=76]').attr('disabled', 'disabled');
                    }
                }
                else if ($(this).attr('data-text') == "Software System") {
                    if ($(this).find('option:selected').text() != "--Select--") {
                        var softwareValueID = $(this).find('option:selected').attr('value');
                        $('select[name=33]').html('');
                        $('select[name=33]').append('<option selected="selected" value="0">--Select--</option>');
                        $.ajax({
                            url: "../CatalogInfo/GetVersionValuesBySoftwareID",
                            type: "POST",
                            async: false,
                            data: { softwarevalueID: softwareValueID },
                            datatype: 'html',
                            success: function (data) {
                                $.each(data, function (key, val) {
                                    $('select[name=33]').append('<option value=' + val["ItemID"] + '>' + val["ItemValue"] + '</option>');
                                });
                            }
                        });
                    }
                }
                //Software System


                $(".catlogDDL").each(function () {
                    var name = $(this).attr('name');
                    if ($('select[name=' + name + ']').find('option:selected').text() == "Other") {
                        otherSelected = true;
                        return false;
                    }
                });
                if (otherSelected) {
                    $(".comments").removeClass('hide');
                }
                else {
                    $(".comments").addClass('hide');
                }


            });
            $(".validationInstructions").blur(function () {
                if (this.value != "") {
                    if ($(this).attr('name') == "64" || $(this).attr('name') == "76" || $(this).attr('name') == "36" || $(this).attr('name') == "74" || $(this).attr('name') == "101") {
                        if (!numberValidation.test(this.value)) {
                            isValid = false;
                            $(this).css('border-color', 'red');
                            $(this).attr('placeholder', 'Please enter integers');
                            $(this).val('');
                        }
                        else {
                            $(this).removeAttr('style');
                            isValid = true;
                        }
                    }
                    else {
                        if (!currncyValidation.test(this.value)) {
                            isValid = false;
                            $(this).css('border-color', 'red');
                            $(this).attr('placeholder', 'Please enter currency');
                            $(this).val('');
                        }
                        else {
                            $(this).removeAttr('style');
                            isValid = true;
                            if ($(this).val().indexOf('$') < 0) {
                                var price = parseFloat($(this).val()).toFixed(2);
                                $(this).val(price);
                            }
                        }
                    }
                }
                else {
                    isValid = true;
                }
            });
            $(".emailValidation").live('blur', function () {
                var emailFiedText = this.value;
                if (emailFiedText != "" && emailFiedText != undefined) {
                    if (!emailRegex.test(emailFiedText)) {
                        isValid = false;
                        $(this).css('border-color', 'red');
                        $(this).attr('placeholder', 'Please enter valid email address');
                        $(this).val('');
                    }
                    else {
                        $(this).removeAttr('style');
                        isValid = true;
                    }
                }
                else {
                    isValid = true;
                }
                ValidationCatalogProfile();
                if ($('#hdnEmailSelected').val() == "true" && $('[name=69]').val() == "") {
                    $('label[data-id=69]').parent().find('.starDisplay').removeClass('hide');
                }
            });

            $(".CheckValidations").live('click', function (e) {
                if (!isValid) {
                    return false;
                }
            });
            if ($('#hdnEmailSelected').val() == "true" && $('[name=69]').val() == "") {
                $('label[data-id=69]').parent().find('.starDisplay').removeClass('hide');
            }
            function ValidationCatalogProfile() {
                var catalogInsertstatus = $("#hdnCatalogInsertStatus").val();
                var validationStatus = true;
                $('.starDisplay').addClass('hide');
                var catalogProfileValidationArray = $("#hdnCatalogMappingText").val().split(";");
                $("#tblCatalogInfo").find('tr').each(function () {
                    if ($(this).find('td:first-child input[type=checkbox]').is(':checked')) {
                        var currentCatalogText = $(this).find('td:first-child input[type=checkbox]').attr('name');
                        $.each(catalogProfileValidationArray, function (i) {
                            if (catalogProfileValidationArray[i].split("=")[0].split('-')[0] == currentCatalogText) {
                                var subjectChecked = false;
                                var dataid;
                                if ($('[name^=' + catalogProfileValidationArray[i].split("=")[1] + ']').hasClass("Subject")) {
                                    $('.Subject').each(function () {
                                        if ($(this).is(':checked')) {
                                            subjectChecked = true;
                                        }
                                        dataid = catalogProfileValidationArray[i].split("=")[1];
                                    });
                                }
                                if (($('[name="' + catalogProfileValidationArray[i].split("=")[1] + '"]').val() == "" || $('[name="' + catalogProfileValidationArray[i].split("=")[1] + '"]').val() == "0")) {
                                    //validationStatus = false;
                                    //$('label[data-id=' + catalogProfileValidationArray[i].split("=")[1] + ']').parent().find('.starDisplay').removeClass('hide');

                                    if ((catalogProfileValidationArray[i].split("=")[1] == "69")) {
                                        if ($('[name=13]').find('option:selected').text() == 'Email' || $('[name=13]').find('option:selected').text() == 'Download') {
                                            validationStatus = false;
                                            $('label[data-id=' + catalogProfileValidationArray[i].split("=")[1] + ']').parent().find('.starDisplay').removeClass('hide');
                                        }
                                        else {
                                            $('label[data-id=' + catalogProfileValidationArray[i].split("=")[1] + ']').parent().find('.starDisplay').addClass('hide');
                                        }
                                    }
                                    else {
                                        validationStatus = false;
                                        $('label[data-id=' + catalogProfileValidationArray[i].split("=")[1] + ']').parent().find('.starDisplay').removeClass('hide');
                                    }


                                }
                                if (subjectChecked == false) {
                                    $('label[data-id=' + dataid + ']').parent().find('.starDisplay').removeClass('hide');
                                }

                            }

                        });
                    }
                });

                if (catalogInsertstatus == "true") {
                    $("#hdnCatalogInsertStatus").val('');
                    if (!validationStatus) {
                        $("#catalogInsertStatus").text("InComplete").css('color', 'red');
                    }

                }
            }
        }
        //Cataloging Script-----------------------------------------------------------------------------------------End

        //Common Script ----------------------------------------------------------------------------------------Start
        function CommonScript() {
            $('.popupClose').live('click', function () {
                $.magnificPopup.close();
            });
            $("#Barcode").focus();
            var pageTitle = $('#hdnPageTitle').val();
            var customerID = $('#hdnCustomerID').val();
            var quoteID = $('#hdnQuoteID').val();
            var quoteType = $('#hdnquoteType').val();
            var groupViewData = $('#hdnViewDataGroupID').val();

            if (pageViewTitle == "About Us" || pageViewTitle == "ContactPenworthy") {
                //  $('#HRMenus').addClass('hide');
            }

            $(document).keyup(function (e) {
                if (e.keyCode == 27) { $('#loadingSetView').html(''); }
            });

            $(".ordernow").live("click", function () {
                $("#btnOrder").removeAttr('disabled', 'disabled');
                $("#btnOrder").removeClass('disabled');
                $("#btnOrder").removeClass('div-disable');
                if ($("#SCItemCount sup").text().trim() == "0" || $("#lblTotalTitles").text() == "0") {
                    $("#btnOrder").attr('disabled', 'disabled');
                    $("#btnOrder").addClass('div-disable');
                }
            });

            $('.popup-with-zoom-anim').magnificPopup({
                type: 'inline',
                fixedContentPos: false,
                fixedBgPos: true,
                overflowY: 'auto',
                closeBtnInside: true,
                preloader: false,
                midClick: true,
                removalDelay: 300,
                mainClass: 'my-mfp-zoom-in',
            });


            //Javascript for updating the DW and Cart Count in content Page
            var scItemsCount = $('#hdnSCItemCount').val();
            var dwItemsCount = $('#hdnDWItemCount').val();
            $('#SCItemCount sup').html(scItemsCount);
            $('#DWItemCount sup').html(dwItemsCount);
            var ScCount = scItemsCount;
            $('.mfp-close').on("click", function () {
                $('#loadingView').html('');
                $("#loadingSetView").html('');
                $('#CreateQuoteView').html('');
                // $('#ItemListView').html('');
                $('.divDW').find('table').find('th:nth-child(' + (11) + '),td:nth-child(' + (11) + ')').removeClass('hide');
                $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr('checked', true);
                var id = $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr("data-statusid");
                QuoteLinksDisable(id);
                $('#DivNotice').addClass('hide');
            });


            $('.BtnDeleteCancel, .btncancel').live("click", function () {
                $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx').attr('checked', false);
                $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr('checked', true);
                var id = $('#tblQuote').find('tbody').find('tr:first-child').find('td').find('#assignChkBx').attr("data-statusid");
                if ($('#tblQuote').find('tbody').find('tr').length == 0) {
                    $(".disablekpl").attr('disabled', 'disabled');
                    $(".disablecategory").attr('disabled', 'disabled');
                    $(".disbalmerge").css("color", "#bebebe").removeAttr('href');
                    $(".disableSubmit").css("color", "#bebebe").removeAttr('href');
                    $('.disableRapidEntry').css("color", "#bebebe").removeAttr('href');
                }
                $.magnificPopup.close();
            });

            $('.uncheck').live('click', function () {
                var btnid = this.id;
                var itemIdsList = [];
                var itemid = $(this).attr('data-itemid');
                var productlinePro = '';
                var quoteID = $('#hdnQuoteID').val();
                var type = '';
                if ($('#hdnGroupName').val() == "Preview") {
                    type = $('#hdnGroupName').val();
                }
                if (btnid == "uncheckall") {
                    $('#dvSet-' + itemid).find('.group1').each(function () {
                        if (!$(this).hasClass('div-disable')) {
                            var id = $(this).attr('value');
                            var leveltype = itemIdsList.push($(this).attr('value'));
                        }
                    });
                }
                $.ajax({
                    url: "../ItemContainerPartial/DeleteItemFromQuote",
                    type: "POST",
                    data: { quoteID: quoteID, itemID: itemIdsList.toString(), type: type },
                    datatype: 'html',

                    success: function (data) {


                        var itemId = data;
                        var tableid;
                        var quantity = 0;

                        var levelType = [];
                        for (var i in itemIdsList) {
                            var itemvalue = itemIdsList[i];
                            $('div:[id="carttext-' + itemvalue + '"]').addClass('hide').hide();
                            $('div:[id="carttextlink-' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttext-+' + itemvalue + '"]').addClass('hide').hide();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttextlink-+' + itemvalue + '"]').removeClass('hide').show();
                            $('input[type=checkbox][value=' + itemvalue + ']').prop('checked', false);
                            $('div:[id="quantitytext-' + itemvalue + '"]').addClass('hide').hide();

                            $('.popup[id=' + itemvalue + ']').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg').attr('data-value', 'Null');
                            $('.popup[id=' + itemvalue + ']').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                            $('.popup[id=' + itemvalue + ']').find('.NoButton').attr('src', '../Images/YesNoMaybe/No_Round.jpg').attr('data-value', 'Null');

                            $('div:[id="divCartStatus-' + itemvalue + '"]').children('div').children('a').find('.listview.YesButton').attr('src', '../Images/YesNoMaybe/YesNew.jpg').attr('data-value', 'Null');
                            $('div:[id="divCartStatus-' + itemvalue + '"]').children('div').children('a').find('.listview.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                            $('div:[id="divCartStatus-' + itemvalue + '"]').children('div').children('a').find('.listview.NoButton').attr('src', '../Images/YesNoMaybe/No.jpg').attr('data-value', 'Null');


                            var $trrow = null;
                            if (pageTitle == "QuoteView") {
                                tableid = '#QuoteViewgrid';
                                $trrow = $('#QuoteViewgrid').find('tbody').find('tr[id="-' + itemvalue + '"]');
                                quantity = $trrow.find('input[type=text][id=' + itemvalue + ']').attr('value');
                                productlinePro = $trrow.find('input[type=text][id=' + itemvalue + ']').attr('data-productline');
                            } else if (pageTitle == "ShoppingCart" || pageTitle == "DecisionWizard") {
                                tableid = '#grid';
                                $trrow = $('#grid').find('tbody').find('tr[id="-' + itemvalue + '"]');
                                quantity = $trrow.find('input[type=text][id=' + itemvalue + ']').attr('value');
                                productlinePro = $trrow.find('input[type=text][id=' + itemvalue + ']').attr('data-productline');
                                if (pageTitle == "DecisionWizard") {
                                    quantity = 0;
                                }
                            }
                            $('input[type=text][id=' + itemvalue + ']').attr('value', 1);

                            if (quantity != undefined && $trrow != null) {
                                levelType = GetLevelTYpe($trrow);
                                $trrow.remove();
                                TotalBooks();
                                TotalTitles();
                                if (quantity != undefined && quantity != 0) {
                                    UpdateCatalogQuantityItemPrice(-(quantity), '', tableid, '', levelType, productlinePro);
                                }
                            }

                            $('span[id=ldwText-' + itemvalue + ']').html("");

                        }
                        if ($("#lblTotalTitles").text() == "0") {
                            $("#btnOrder").attr('disabled', 'disabled');
                            $("#btnOrder").addClass('div-disable');
                        }
                        if ($('.gridscrollContent tr').length > 0 || $('.gridQuotescrollContent tr').length > 0 || $('.gridFourBodyContent tr').length > 0) {
                            CalculatePrice();
                            if ($('#Include_Cataloging').prop('checked') == false) {
                                $("#grid tr[id='-Catalog']").addClass("hide");
                                $("#QuoteViewgrid  tr[id='-Catalog']").addClass("hide");
                            }
                        }
                        else {
                            UpdateCatalogQuantityItemPrice(0, '', tableid, '', '')
                            $("#grid tr[id='-Catalog']").addClass("hide");
                            $("#QuoteViewgrid  tr[id='-Catalog']").addClass("hide");

                            $('#SalesTax').html('$0.00');
                            $('#totalPrice').html('$0.00');
                            $('#lblTotalPrice').html('$0.00');
                            $('#countprice').html('0 items $0.00');
                        }
                        $.ajax({
                            url: '../ShoppingCart/GetlstScDetailsbyQuoteID',
                            type: "POST",
                            data: { QuoteID: quoteID, type: type },
                            datatype: 'json',
                            success: function (data) {
                                var AllCount;
                                var noCount
                                    , yesCount
                                    , maybeCount
                                    , newCount,
                                        yesTotalPrice;
                                //var model = data;
                                AllCount = data[6];
                                noCount = data[3];

                                yesCount = data[2];
                                maybeCount = data[4];
                                newCount = data[5];
                                yesTotalPrice = data[8];

                                $('.lableValTD').each(function () {
                                    if (this.id == "0") $(this).html('(' + AllCount + ')');
                                    if (this.id == "1") $(this).html('(' + yesCount + ')');
                                    if (this.id == "2") $(this).html('(' + noCount + ')');
                                    if (this.id == "3") $(this).html('(' + maybeCount + ')');
                                    if (this.id == "5") $(this).html('(' + newCount + ')');
                                });
                                $("#lblYesPrice").text('$' + parseFloat(yesTotalPrice).toFixed(2));
                                var itemCount = data[0];
                                $('#lblTotalItemTitles').html(JSON.stringify(data[0]).replace(/"/g, ''));
                                var price = JSON.stringify(data[1]).replace(/"/g, '');
                                $('#lblTotalItemPrice').html('$' + parseFloat(price).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,"));
                                $('#SCItemCount sup').html(itemCount);
                                $('#SCItemPrice a').html('$' + parseFloat(price).toFixed(2));
                                if (pageTitle == "KPL") {
                                    //$.each(model.KPLItemListVM, function (i, val) {
                                    //    ItemPrice(val['ItemID'], "Inc");
                                    //});
                                    $('#itemcount').text(" : " + JSON.stringify(data[0]).replace(/"/g, ''));
                                    $('#totalprice').html(":   $" + parseFloat(price).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,"));
                                }
                            }
                        });
                    }
                });
            });



            $('#btnaddall').live('click', function () {
                var btnid = this.id;
                var quoteID = $('#hdnQuoteID').val();
                var itemIdsList = [];

                $('#dvSet-1').find('.group1').each(function () {
                    if (!$(this).hasClass('div-disable')) {
                        itemIdsList.push($(this).attr('value'));
                    }
                });

                $.ajax({
                    url: "../ItemListView/UpdateStatusofAllItems",
                    type: "POST",
                    data: { selectbtnid: btnid, itemID: itemIdsList.toString(), quoteID: quoteID, type: type },
                    //  selectbtnid: DWselectionID, itemID: itemIdsList.toString(), quoteID: quoteID, type: type
                    datatype: 'html',
                    success: function (data) {
                        for (var i in itemIdsList) {
                            var itemvalue = itemIdsList[i];
                            $('div:[id="carttext-' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="carttextlink-' + itemvalue + '"]').addClass('hide').hide();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttext-+' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttextlink-+' + itemvalue + '"]').addClass('hide').hide();
                            $('#SCItemCount sup').html(data.SCItemsCount);
                            $('#SCItemPrice a').html('$' + data.SCItemsPrice.toFixed(2));
                        }
                    }
                });
            });
            $("#grid").tablesorter();
            $('#Excel,.Excel').on("click", function (e) {
                //  ExportExcel();
            });
            var urlpath = $("#hdnlogoPath").val();
            function ExportExcel() {
                $("#divTableData").html($('#divSCtable').html());

                // $("#divTableData").find("table thead tr:first").css("border-bottom", "1px solid black");
                $("#divTableData").find("table thead th:last").remove();
                $.each($("#divTableData").find("table tbody tr"), function () {
                    $(this).find("td:last").remove();
                    $.each($(this).find("td"), function () {
                        if (this.id == "Title") {
                            $(this).find("img").remove();
                            $(this).text($(this).find("a").attr('href', urlpath));
                        }
                        if (this.id == "Quantity") {
                            $(this).text($(this).find("div div").eq(1).find(".Quantity").val());
                        }
                    })
                });
                $.each($("#divTableData").find("table tfoot tr"), function () {
                    if (pageTitle != "DecisionWizard") {
                        if ($(this).attr('id') == 'trforisbn') {
                            $(this).remove();
                        }
                        if ($('#Include_Cataloging').prop('checked') == false) {
                            if ($(this).attr('id') == '-Catalog') {
                                $(this).remove();
                            }
                        }
                    }
                    $.each($(this).find("th"), function () {

                        if (this.id == "Quantity") {
                            $(this).text($(this).find("div div").eq(1).find(".Quantity").val());
                        }
                    })
                });
                var data = $("#tblHeader").html();
                var rowStatus = ($("#divTableData").find("table tbody tr").length - $("#divTableData").find("table tbody tr[id=-Catalog]").length) != 0 ? true : false;
                if (rowStatus) {
                    data = data + $('#divTableData').html();
                }
                window.open('data:application/vnd.ms-excel,' + encodeURIComponent(data));
                $("#divTableData").html('');
            }
            //  $('#lblTotalPrice').html($('#hdnTotalPrice').val());
            $('#lblTotalTitles').html($('#hdnTotalItems').val());

            var loginPath = $('#hdnLoginPath').val();
            var isSingle = $('#hdnViewType').val() == "SingleView" ? true : false;
            var viewSingleItemPath = $('#hdnViewSingleItemPath').val();
            var type = $('#hdnPageType').val();
            if (pageTitle == "ItemListView") {
                if ($('#hdnGroupName').val() == "Preview") {
                    type = $('#hdnGroupName').val();
                }
            }
            $('.ItemDetailSeries').live("click", function () {
                isSingle = $('#hdnViewType').val() == "SingleView" ? true : false;
                viewSingleItemPath = pageTitle == "ItemListView" && isSingle ? $('#hdmItemListViewSinglePath').val() : $('#hdnViewSingleItemPath').val();
                if (pageTitle == "ItemListView" && isSingle) {
                    $('#small-dialog').css("height", "640px");
                    $('#small-dialog').css("width", "960px").css('margin', '0 auto 0');

                    var itemId = this.title;
                    $.ajax({
                        url: viewSingleItemPath,
                        type: "POST",
                        data: { itemID: this.title, QuoteID: quoteID, type: type },
                        datatype: 'html',
                        success: function (data) {
                            $('#loadingSetView').html('');
                            $('#loadingSetView').html(data);

                            $('.popup-with-zoom-anim').magnificPopup({

                                type: 'inline',
                                fixedContentPos: false,
                                fixedBgPos: true,
                                overflowY: 'auto',
                                closeBtnInside: true,
                                preloader: false,
                                midClick: true,
                                removalDelay: 300,
                                mainClass: 'my-mfp-zoom-in',
                            });
                        }

                    });
                }
                else {
                    var itemId = this.title;
                    $('#small-dialog').css("height", "640px");
                    $('#small-dialog').css("width", "960px").css('margin', '0 auto 0');
                    $.ajax({
                        url: viewSingleItemPath,
                        type: "POST",
                        data: { itemID: this.title, QuoteID: quoteID, type: type },
                        datatype: 'html',
                        success: function (data) {
                            $('#loadingSetView').html('');
                            $('#loadingSetView').html(data);

                            $('#BarcodeloadingSetView').html('');
                            $('#BarcodeloadingSetView').html(data);
                            $('.popup-with-zoom-anim').magnificPopup({
                                type: 'inline',
                                fixedContentPos: false,
                                fixedBgPos: true,
                                overflowY: 'auto',
                                closeBtnInside: true,
                                preloader: false,
                                midClick: true,
                                removalDelay: 300,
                                mainClass: 'my-mfp-zoom-in',
                            });

                        },
                    });
                }
            });

            var totalPrice;
            var AddCartPath = $('#hdnAddCartPath').val();
            var quoteID = $('#hdnQuoteID').val();
            // var jsnModelData = JSON.parse($('#hdnGroupViewModel').val());
            $('.AddCart').unbind('mousedown').live('mousedown', function (e) {
                var custroleexist = $('#hdnCustRoleExists').val();
                var btnid = this.id;
                var itemIdsList = [];
                var itemid = $(this).attr('data-itemid');
                var type = $('#hdnPageType').val();
                if ($('#hdnGroupName').val() == "Preview") {
                    type = $('#hdnGroupName').val();
                }
                var AllCount;
                var noCount
                    , yesCount
                    , maybeCount
                    , newCount;
                if (btnid == "5") {
                    //  $(this).attr("style", "border-radius: 60%; height: 60px; width: 60px; background-color: white; border: solid; text-align: center; font-size: 12px; font-weight: bold;");
                    $('#dvSet-' + itemid).find('.group1').each(function () {
                        if (!$(this).hasClass('div-disable')) {
                            itemIdsList.push($(this).attr('value'));
                        }
                    });
                }
                else {
                    // $(this).attr('style', 'background-color: white!important;color: #2e92cf!important;padding:0px;font-family:Segoe UI_,Open Sans,Verdana,Arial,Helvetica,sans-serif;');
                    var Itemid = $(this).attr('data-itemid');
                    itemIdsList.push(Itemid);
                }
                var DWselectionID = "1";
                if (custroleexist == false) {
                    DWselectionID = "5";
                }

                $.ajax({
                    url: AddCartPath,
                    type: "POST",
                    async: false,
                    data: { selectbtnid: DWselectionID, itemID: itemIdsList.toString(), quoteID: quoteID, type: type },
                    datatype: 'html',
                    success: function (data) {
                        var model = data;
                        AllCount = model.SelectionCount;
                        noCount = model.noOfNoCount;
                        yesCount = model.noOfYesCount;
                        maybeCount = model.noOfMaybeCount;
                        newCount = model.noOfNewCount;

                        for (var i in itemIdsList) {

                            var itemvalue = itemIdsList[i];
                            if (Issingleview) {

                                $('.popup[id=' + itemvalue + ']').find('.YesButton').attr('src', '../Images/YesNoMaybe/Yes_Round.jpg').attr('data-value', 'Null');
                                $('.popup[id=' + itemvalue + ']').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                $('.popup[id=' + itemvalue + ']').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');
                            }
                            else {
                                $('.popup[id=' + itemvalue + ']').find('.YesButton').attr('src', '../Images/YesNoMaybe/Yes_Round.jpg').attr('data-value', 'Null');
                                $('.popup[id=' + itemvalue + ']').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                $('.popup[id=' + itemvalue + ']').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');

                                $('div:[id="divCartStatus-' + itemvalue + '"]').children('div').children('a').find('.listview.YesButton').attr('src', '../Images/YesNoMaybe/Yes.jpg').attr('data-value', 'Null');
                                $('div:[id="divCartStatus-' + itemvalue + '"]').children('div').children('a').find('.listview.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                                $('div:[id="divCartStatus-' + itemvalue + '"]').children('div').children('a').find('.listview.NoButton').attr('src', '../Images/YesNoMaybe/NoNew.jpg').attr('data-value', 'Null');


                            }
                            $('a[id=' + itemvalue + '][value=1]').children('img').attr('data-value', 'Yes');
                            $('div:[id="carttext-' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="carttextlink-' + itemvalue + '"]').addClass('hide').hide();
                            $('div:[id="quantitytext-' + itemid + '"]').removeClass('hide').show();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttext-+' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttextlink-+' + itemvalue + '"]').addClass('hide').hide();
                            $('input[type=checkbox][value=' + itemvalue + ']').prop('checked', true);

                        }
                        $('.lableValTD').each(function () {
                            if (this.id == "0") $(this).html('(' + AllCount + ')');
                            if (this.id == "1") $(this).html('(' + yesCount + ')');
                            if (this.id == "2") $(this).html('(' + noCount + ')');
                            if (this.id == "3") $(this).html('(' + maybeCount + ')');
                            if (this.id == "5") $(this).html('(' + newCount + ')');
                        });
                        $("#lblYesPrice").text('$' + model.YesTotalPrice.toFixed(2));
                        $('#SCItemCount sup').html(model.SCItemsCount);
                        if ($("#SCItemCount sup").text() != '0') {
                            //   alert($("#SCItemCount sup").text());
                            $('#btnOrder').removeClass('disabled');
                            $('#btnOrder').removeAttr('disabled');
                        }
                        $('#SCItemPrice a').html('$' + model.SCItemsPrice.toFixed(2));
                        $('#DWItemCount sup').html(AllCount);

                        $.each(model.KPLItemListVM, function (i, val) {
                            $('span[id=ldwText-' + val["ItemID"] + ']').html("LDW: " + model.KPLItemListVM[0]["DWDate"]);
                        });
                        //$(".bgcolor").text("In Cart").remove("pointer-events", "none");
                        scItemsCount = model.SCItemsCount
                        var calcpercentage = ((parseInt(yesCount)) / (parseInt(AllCount))) * 100;
                        var pb1 = $('#pb1').progressbar();
                        pb1.progressbar('value', parseInt(calcpercentage));
                        UpdateAddCartSeriesInformation(model, pageTitle, itemIdsList);

                    },
                    error: function (data) {
                        alert(data);
                    }

                });
            });
            var AddSeriesPath = $('#hdnAddSeriesPath').val();
            $('.AddSeries').unbind('click').live('click', function (e) {
                var itemid = $(this).attr('data-itemid');
                $.ajax({
                    url: '../ItemListView/AddSeriesByItemID',
                    type: "POST",
                    data: { itemID: itemid, quoteID: quoteID },
                    datatype: 'html',
                    success: function (data) {
                        var model = data;
                        var itemTextIdsList = [];
                        var itemLinkIdsList = [];
                        var itemIdsList = [];
                        $('#SCItemCount sup').html(model.SCItemsCount);
                        $('#dvSeries').find("div[id^='carttext-']").each(function () {
                            itemTextIdsList.push(this.id);

                        }).removeClass('hide').show();
                        $('#dvSeries').find("div[id^='carttextlink-']").each(function (i) {
                            itemLinkIdsList.push(this.id);
                            itemIdsList.push(itemLinkIdsList[i].split('-')[1]);
                        }).hide();
                        for (var i in itemTextIdsList) {
                            $('.' + itemTextIdsList[i]).removeClass('hide').show();
                        }
                        for (var i in itemLinkIdsList) {
                            $('.' + itemLinkIdsList[i]).hide();
                        }
                        UpdateAddCartSeriesInformation(model, pageTitle, itemIdsList);
                    }
                });
            });



            var AddCollectionPath = $('#hdnCollectionPath').val();
            $('.AddCollection').unbind('mousedown').live('mousedown', function (e) {
                var custroleexist = $('#hdnCustRoleExists').val();
                var btnid = this.id;
                var itemIdsList = [];
                var SelectedFilters = [];
                $(".childCheckBox").each(function () {
                    if ($(this).is(':checked')) {
                        SelectedFilters.push($(this).attr('data-groupId'));
                    }
                });
                var groupID = $('#hdnPkgID').val();
                var DWselectionID = "1";
                if (custroleexist == false) {

                    DWselectionID = "5";
                }
                $.ajax({
                    // async: false,
                    url: AddCollectionPath,
                    type: "POST",
                    data: { groupID: groupID, quoteID: quoteID, selectedFilters: SelectedFilters.toString() },
                    datatype: 'html',
                    success: function (data) {
                        var model = data;
                        AllCount = model.SelectionCount;
                        noCount = model.noOfNoCount;
                        yesCount = model.noOfYesCount;
                        maybeCount = model.noOfMaybeCount;
                        newCount = model.noOfNewCount;
                        var ItemIDS = model.ItemIDS;
                        var ItemList = [];
                        itemIdsList = $.makeArray(ItemIDS).toString().split(',');

                        $.each(itemIdsList, function (i, val) {
                            var itemvalue = val;

                            $('div:[id="carttext-' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="carttextlink-' + itemvalue + '"]').hide();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttext-+' + itemvalue + '"]').removeClass('hide').show();
                            $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttextlink-+' + itemvalue + '"]').hide();
                        });

                        $('.lableValTD').each(function () {
                            if (this.id == "0") $(this).html('(' + AllCount + ')');
                            if (this.id == "1") $(this).html('(' + yesCount + ')');
                            if (this.id == "2") $(this).html('(' + noCount + ')');
                            if (this.id == "3") $(this).html('(' + maybeCount + ')');
                            if (this.id == "5") $(this).html('(' + newCount + ')');
                        });
                        var itemtotalPrice = 0;
                        var itemCount = 0;
                        $.each(model.KPLItemListVM, function (i, val) {
                            itemtotalPrice = itemtotalPrice + parseFloat(val['Price']);
                            itemCount = itemCount + 1;
                        });
                        itemtotalPrice = parseFloat($("#lblTotalItemPrice").text().replace('$', '').replace(',', '')) + itemtotalPrice;
                        $("#lblTotalItemTitles").text(parseInt($("#lblTotalItemTitles").text().replace(",", "")) + itemCount);
                        $("#lblTotalItemPrice").text('$' + itemtotalPrice.toFixed(2));
                        $('#SCItemCount sup').html(model.SCItemsCount);
                        $('#DWItemCount sup').html(AllCount);
                        $('#SCItemPrice a').html('$' + model.SCItemsPrice.toFixed(2));
                        scItemsCount = model.SCItemsCount
                        var calcpercentage = ((parseInt(AllCount) - parseInt(newCount)) / (parseInt(AllCount))) * 100;
                        var pb1 = $('#pb1').progressbar();
                        pb1.progressbar('value', parseInt(calcpercentage));
                        if ($("#SCItemCount sup").text() != '0') {
                            $('#btnOrder').removeClass('disabled');
                            $('#btnOrder').removeAttr('disabled');
                        }
                    },
                    error: function (data) {
                        alert(data);
                    }

                });

            });


            var includeCatalogPath = $("#hdnCatalogPath").val();
            $('.includeCatalog').live('click', function () {
                var totalPrice = 0;
                var blnchk = $('.includeCatalog').is(':checked');
                if (blnchk == true) {
                    includeCatalog = true;
                    $.ajax({
                        url: "../QuoteView/UpdateIncludeCatalogStatus",
                        type: "POST",
                        data: { quoteID: quoteID, IncludeCatalogStatus: includeCatalog },
                        datatype: 'html',
                        success: function (data) {
                            if (pageTitle == "ShoppingCart") {
                                if ($('.gridscrollContent tr').length > 0) {
                                    $('#grid tr').each(function () {
                                        var itemquantity = $(this).find('th[id=Quantity]').children('div').children('div').find('input').attr('value');
                                        if ($(this).attr('id') == "-Catalog" && itemquantity != 0) {
                                            $(this).removeClass('hide');
                                            $(this).find('td[id=Del]').html('');
                                        }
                                    });
                                    CalculatePrice();
                                }
                                //TotalBooks();
                                //TotalTitles();

                            }
                            if (pageTitle == "QuoteView") {
                                if ($('.gridQuotescrollContent tr').length > 0) {
                                    $('#QuoteViewgrid tfoot>tr').each(function () {
                                        var itemquantity = $(this).find('th[id=Quantity]').children('div').children('div').find('input').attr('value');
                                        if ($(this).attr('id') == "-Catalog" && itemquantity != 0) {
                                            $(this).removeClass('hide');
                                        }
                                    });
                                    CalculatePrice();
                                }
                            }
                        },
                    });

                }
                else {
                    includeCatalog = false;
                    $.ajax({
                        url: "../QuoteView/UpdateIncludeCatalogStatus",
                        type: "POST",
                        data: { quoteID: quoteID, IncludeCatalogStatus: includeCatalog },
                        datatype: 'html',
                        success: function (data) {
                            if (pageTitle == "ShoppingCart") {
                                if ($('.gridscrollContent tr').length > 0) {
                                    $('#grid tr').each(function () {
                                        if ($(this).attr('id') == "-Catalog") {
                                            $(this).addClass('hide');
                                        }
                                    });
                                    CalculatePrice();
                                }
                                //TotalBooks();
                                //TotalTitles();

                            }

                            if (pageTitle == "QuoteView") {
                                if ($('.gridQuotescrollContent tr').length > 0) {
                                    $('#QuoteViewgrid tfoot>tr').each(function () {
                                        if ($(this).attr('id') == "-Catalog") {
                                            $(this).addClass('hide');
                                        }
                                    });
                                    CalculatePrice();
                                }
                            }

                        },
                    });

                }
            });
            $(".allowEnterKey").live("keypress", function (e) {
                if (e.keyCode == 13) {
                    if ($('#UserEmail').val() == "") {
                        $("#invaliduname").removeClass("hide");
                    }
                    else if ($('#Password').val() == "") {
                        $("#upass").removeClass("hide");
                        if ($("#invaliduname").hasClass("hide"))
                            $("#forgotPassword").removeClass("hide");

                    }
                    else {
                        $('#login').click();
                    }
                }
            });

            $('.userValidations').live("click", function () {
                $("#invaliduname").addClass("hide");
                if ($('#UserEmail').val() == "") {
                    $("#invaliduname").removeClass("hide");
                }
                if ($('#Password').val() == "") {
                    $("#upass").removeClass("hide");
                    if ($("#invaliduname").hasClass("hide"))
                        $("#forgotPassword").removeClass("hide");
                }
                else {
                    $("#upass").addClass("hide");
                }
                if ($('#UserEmail').val() != "" && $('#Password').val() != "") {
                    if ($("#ddl  option:selected").length > 0) {
                        $("#CustAutoID").val($("#ddl").val());
                    }
                    var email = $('#UserEmail').val();
                    if ($("#CustAutoID").val() != "") {
                        email = email + "~" + $("#CustAutoID").val();
                    }
                    $.ajax({
                        url: "../Default/UserPreValidations",
                        type: "POST",
                        data: { emailAndCustAutoID: email, pass: $("#Password").val() },
                        datatype: 'html',
                        success: function (data) {
                            if (data == "ValidUser") {
                                $('#loginform').submit();
                            }
                            else {
                                var listItems = "";
                                if (data == "InvalidUserName") {
                                    $("#invaliduname").removeClass("hide");
                                    $("#forgotPassword").addClass('hide');
                                }
                                else if (data == "ChangePasswordFailed") {
                                    $("#ddl").html('');
                                    $("#UserEmail").attr('readonly', 'true');
                                    $('#Password').val('');
                                    // $('#small-dialog').css("height", "233px");
                                    //  $('#small-dialog').css("width", "405px").css('margin-left', '42%');
                                    $("#loginfieldset").removeClass('hide');
                                    $("#invaliduname").addClass('hide');
                                    $("#forgotPassword").removeClass("hide");//Added by Amer
                                    //$("#userErrorInfo").removeClass("hide");
                                    $("#loginCustomerDetails").addClass('hide');
                                    userID = $("#CustAutoID").val();
                                    $("#CustAutoID").val("");
                                    // $("#hdnLoginStatus").val("false");
                                }
                                else {
                                    if (data.length > 1 && data[0]["ItemID"] > 0) {
                                        $("#loginfieldset").addClass('hide');
                                        //$('#small-dialog').css("height", "130px");
                                        $('#loginCustomerDetails').removeClass('hide');
                                        $("#loginfieldset").addClass('hide');
                                        $("#manageCustomerDetails").addClass('hide');
                                        $("#forgotPasswordDetails").addClass("hide");
                                        $.each(data, function (i, val) {
                                            listItems += "<option value='" + val["ItemID"] + "'>" + val["ItemValue"] + "</option>";
                                        });
                                        $("#ddl").html(listItems);
                                        // $('#small-dialog').css("width", "300px").css('margin-left', '42%');
                                    }

                                    else {
                                        $('#manageCustomerDetails').removeClass('hide');
                                        $("#loginCustomerDetails").addClass('hide');
                                        // $('#small-dialog').css("height", "235px");
                                        //  $('#small-dialog').css("width", "445px").css('margin-left', '42%');
                                        // $("#Password").attr('name', 'oldpass');
                                        $("#manageCustomerDetails").html(data);
                                        $("#loginfieldset").addClass('hide');
                                        $("#forgotPasswordDetails").addClass("hide");
                                    }
                                }
                            }
                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
            });
            $('#manageLiveCust').live("click", function () {
                if ($("#NewPassword").val() != '' && ($("#NewPassword").val() == $("#ConfirmPassword").val())) {
                    if (($("#CustAutoID").val() == "" && $("#CustAutoID").val() != undefined)) {
                        $("#CustAutoID").val($("#ddl").val());
                    }
                }
                else {
                    $("#NewPassword").val('');
                    $("#ConfirmPassword").val('');
                    $("#invaliduname").addClass("hide");
                    return false;
                }

            });
            $('#manageInfo').live("click", function () {
                if ($('#OldPassword').length > 0) {
                    $('#OldPassword').val($('[name=Password]').val());
                }
                if ($("#NewPassword").val() != '' && ($("#NewPassword").val() == $("#ConfirmPassword").val())) {
                    var email = $('#UserEmail').val();
                    if ($("#CustAutoID").val() != "") {
                        email = email + "~" + $("#CustAutoID").val();
                    }
                    if ($("#OldPassword").attr("readonly") == "readonly") {
                        $('#loginform').submit();
                    }
                    else {
                        $.ajax({
                            url: "../Default/UserPreValidations",
                            type: "POST",
                            data: { emailAndCustAutoID: emailAndId, pass: $("[name=Password]").val(), type: "checkoldpass" },
                            datatype: 'html',
                            success: function (data) {
                                if (data == "ValidUser") {
                                    $('#loginform').submit();
                                }
                                else {
                                    $("#manageCurrPass").removeClass("hide");
                                }
                            },
                            error: function (data) {
                                alert(data);
                            }
                        });
                    }
                }
                else {
                    $("#NewPassword").val('');
                    $("#ConfirmPassword").val('');
                    $("#manageCurrPass").addClass("hide");
                    return false;
                }

            });
            $(".forgorPasserror").live("click", function () {
                if ($(this).text() != "Cancel") {
                    if ($(this).hasClass("conformDialog")) {
                        $("#forgotpassWizard").addClass("hide");
                        $("#conformDialog").removeClass("hide");
                    }
                    else {
                        $.ajax({
                            url: "../Default/SendForgortPassword",
                            type: "POST",
                            data: { emailAndUserId: emailAndId },
                            datatype: 'html',
                            success: function (data) {
                                if (data == "ChangePasswordFailed") {
                                    $("#loginfieldset").addClass('hide');
                                    $("#manageCustomerDetails").addClass('hide');
                                    $("#loginCustomerDetails").addClass('hide');
                                    $("#forgotPasswordDetails").css("margin", "23px 0px 0px 0px");
                                    $("#forgotPasswordDetails").css("text-align", "center");
                                    $("#forgotPasswordDetails").html("Invalid Email Address");
                                }
                                else if (data == "ValidUser") {
                                    $("#loginfieldset").addClass('hide');
                                    $("#manageCustomerDetails").addClass('hide');
                                    $("#loginCustomerDetails").addClass('hide');
                                    $("#forgotPasswordDetails").css("margin", "20px 0px 0px 9px");
                                    $("#forgotPasswordDetails").html("Email has been Successfully sent.Please check your email to login");
                                }

                            },
                            error: function (data) {
                                alert(data);
                            }
                        });
                    }

                }
                else {
                    $("#loginCustomerDetails").addClass('hide');
                    $("#loginfieldset").removeClass("hide");
                    $("#manageCustomerDetails").addClass("hide");
                    // $("#userErrorInfo").addClass('hide');

                    $("#forgotPasswordDetails").addClass("hide");
                    //  $('#small-dialog').css("height", "215px");
                    //$('#small-dialog').css("width", "405px").css('margin-left', '42%');
                }
                userID = '';

            });
            var userID;
            var emailAndId;
            var type;
            $(".forgorPass").live('click', function () {
                var emailonly;
                if ($("#UserEmail").length > 0) {
                    emailAndId = $("#UserEmail").val();
                    emailonly = $("#UserEmail").val();
                    if ($("#ddl  option:selected").length > 0) {
                        emailAndId = emailAndId + "~" + $("#ddl").val();
                    }
                }
                if ($(this).text() == "Change Password") {
                    type = "changepassword";
                    $("#Password").attr('name', 'oldpass');
                }
                $.ajax({
                    url: "../Default/ForgotPasswordDetails",
                    type: "POST",
                    datatype: 'html',
                    data: { emailAndCustAutoID: emailAndId, type: type },
                    success: function (data) {
                        if (data != "InvalidUserName") {
                            if (data.length > 1 && data[0]["ItemID"] > 0) {
                                $("#loginfieldset").addClass('hide');
                                //$('#small-dialog').css("height", "130px");
                                $('#loginCustomerDetails').removeClass('hide');
                                $("#loginfieldset").addClass('hide');
                                $("#manageCustomerDetails").addClass('hide');
                                $("#forgotPasswordDetails").addClass("hide");
                                var listItems = '';
                                $.each(data, function (i, val) {
                                    listItems += "<option value='" + val["ItemID"] + "'>" + val["ItemValue"] + "</option>";
                                });
                                $("#customerSelectionConfirmation").removeClass("userValidations");
                                $("#customerSelectionConfirmation").addClass("forgorPass");
                                $("#ddl").html(listItems);
                                // $('#small-dialog').css("width", "300px").css('margin-left', '42%');
                            }
                            else {
                                $("#ddl").html('');
                                $("#forgotPasswordDetails").html(data);
                                $("#ForgotEmail").val(emailonly);
                                $("#loginfieldset").addClass('hide');
                                $("#loginCustomerDetails").addClass('hide');
                                $("#manageCustomerDetails").addClass('hide');
                                $("#forgotPasswordDetails").removeClass("hide");
                                if (type == "changepassword") {
                                    type = "";
                                }
                            }
                        }
                        else {
                            $("#invaliduname").removeClass("hide");
                        }

                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            });
            var deleteItemfromQuotePath = $('#hdnDeletePath').val();
            $('.group2').live("click", function () {
                var ischecked = $(this).is(':checked');
                var levelType = [];
                var itemid = $(this).attr('value');
                var addItemtoQuotePath = "../ItemListView/UpdateKPLBuilderQuote";
                var itemPrice = $(this).attr('data-price');
                var type = '';
                if ($('#hdnGroupName').val() == "Preview") {
                    type = $('#hdnGroupName').val();
                }
                if (Issingleview) {
                    $('a[id=' + itemid + '][value=2]').children('img').attr('src', '../Images/YesNoMaybe/NoNew.jpg').attr('data-value', 'Null');
                    $('a[id=' + itemid + '][value=3]').children('img').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                    $('a[id=' + itemid + '][value=1]').children('img').attr('src', '../Images/YesNoMaybe/Yes.jpg');
                }
                else {
                    $('.popup[id=' + itemid + ']').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg').attr('data-value', 'Null');
                    $('.popup[id=' + itemid + ']').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                    $('.popup[id=' + itemid + ']').find('.NoButton').attr('src', '../Images/YesNoMaybe/No_Round.jpg').attr('data-value', 'Null');

                    $('div:[id="divCartStatus-' + itemid + '"]').children('div').children('a').find('.listview.YesButton').attr('src', '../Images/YesNoMaybe/YesNew.jpg').attr('data-value', 'Null');
                    $('div:[id="divCartStatus-' + itemid + '"]').children('div').children('a').find('.listview.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                    $('div:[id="divCartStatus-' + itemid + '"]').children('div').children('a').find('.listview.NoButton').attr('src', '../Images/YesNoMaybe/No.jpg').attr('data-value', 'Null');

                }
                var url = ischecked ? addItemtoQuotePath : deleteItemfromQuotePath;

                var itemCount = 0;
                $.ajax({
                    url: url,
                    type: "POST",
                    data: { quoteID: quoteID, itemID: itemid, type: type },
                    datatype: 'html',
                    success: function (data) {
                        var itemId = data;
                        var datainput = [];
                        datainput.push(itemid);
                        datainput.push(itemPrice);
                        $.ajax({
                            url: '../ShoppingCart/GetlstScDetailsbyQuoteID',
                            type: "POST",
                            data: { QuoteID: quoteID, type: type },
                            datatype: 'json',
                            success: function (data) {
                                var AllCount;
                                var noCount
                                    , yesCount
                                    , maybeCount
                                    , newCount,
                                        yesTotalPrice;

                                //var model = data;
                                AllCount = data[6];
                                noCount = data[3];
                                yesCount = data[2];
                                maybeCount = data[4];
                                newCount = data[5];
                                yesTotalPrice = data[8];
                                $('.lableValTD').each(function () {
                                    if (this.id == "0") $(this).html('(' + AllCount + ')');
                                    if (this.id == "1") $(this).html('(' + yesCount + ')');
                                    if (this.id == "2") $(this).html('(' + noCount + ')');
                                    if (this.id == "3") $(this).html('(' + maybeCount + ')');
                                    if (this.id == "5") $(this).html('(' + newCount + ')');
                                });

                                $("#lblYesPrice").text('$' + parseFloat(yesTotalPrice).toFixed(2));
                                itemCount = data[0];
                                $('#lblTotalItemTitles').html(JSON.stringify(data[0]).replace(/"/g, ''));
                                var price = JSON.stringify(data[1]).replace(/"/g, '');
                                $('#lblTotalItemPrice').html('$' + parseFloat(price).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,"));
                                $('#SCItemCount sup').html(itemCount);
                                $('#SCItemPrice a').html('$' + parseFloat(price).toFixed(2));
                                if (ischecked) {
                                } else {
                                    $('input[type=checkbox][value=' + itemid + ']').prop('checked', false);
                                    $('div:[id="carttext-' + itemid + '"]').addClass('hide').hide();
                                    $('div:[id="carttextlink-' + itemid + '"]').removeClass('hide').show();

                                    $('div:[id="quantitytext-' + itemid + '"]').addClass('hide').hide();
                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttext-+' + itemid + '"]').addClass('hide').hide();
                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttextlink-+' + itemid + '"]').removeClass('hide').show();

                                    var $trrow;
                                    var tableid;
                                    if (pageTitle == "QuoteView") {
                                        tableid = '#QuoteViewgrid';
                                        $trrow = $('#QuoteViewgrid').find('tbody').find('tr[id="-' + itemid + '"]');
                                    }
                                    else {
                                        tableid = '#grid';
                                        $trrow = $('#grid').find('tbody').find('tr[id="-' + itemid + '"]');
                                    }
                                    var quantity = $trrow.find('input[type=text][id=' + itemid + ']').attr('value');



                                    var productlinePro = $trrow.find('input[type=text][id=' + itemid + ']').attr('data-productline');
                                    leveltype = GetLevelTYpe($trrow);
                                    UpdateCatalogQuantityItemPrice(-(quantity), '', tableid, '', leveltype, productlinePro);
                                    $trrow.remove();
                                    if (pageTitle == "ShoppingCart") {
                                        TotalBooks();
                                    }
                                    TotalTitles();
                                    CalculatePrice()
                                    if ($('#Include_Cataloging').prop('checked') == false) {
                                        $("#grid tr[id='-Catalog']").addClass("hide");
                                        $("#QuoteViewgrid  tr[id='-Catalog']").addClass("hide");

                                    }
                                    if ($("#lblTotalTitles").text() == "0") {

                                        $("#btnOrder").attr('disabled', 'disabled');
                                        $("#btnOrder").addClass('div-disable');
                                    }
                                    $('input[type=text][id=' + itemid + ']').attr('value', 1);
                                    if (pageTitle == "Categories") {
                                        if ($('span[id=ldwText-' + itemid + ']') != undefined) {
                                            $('span[id=ldwText-' + itemid + ']').html("");
                                        }
                                    }

                                    //'.gridscrollContent tr').length == 0 || $('.gridQuotescrollContent tr').length == 0 || $('.gridFourBodyContent tr'
                                    if ($(tableid + ' tbody tr').length == 0) {
                                        UpdateCatalogQuantityItemPrice(0, '', tableid, '', '', '');
                                        CalculatePrice();
                                        $('#totalPrice').html('$0.00');
                                        $('#SalesTax').html('$0.00');
                                        $('#countprice').html(0 + ' items ' + $('#totalPrice').html());
                                        $("#grid tr[id='-Catalog']").addClass("hide");
                                        $("#QuoteViewgrid  tr[id='-Catalog']").addClass("hide");
                                    }
                                    $('#SCItemCount sup').html(itemCount);
                                    $('#SCItemPrice a').html('$' + parseFloat(price).toFixed(2));
                                }
                            }

                        });

                    }
                });
            });

            $('a.NumericUpDownBtns').live("click", function () {
                var id = this.title;
                var qty;
                var quantityValue;
                var tableiD = pageViewTitle == "ShoppingCart" ? '#grid' : '#QuoteViewgrid'
                var incDecType = '';
                var productLinepro = '';
                var type = '';
                if ($('#hdnGroupName').val() == "Preview") {
                    type = $('#hdnGroupName').val();
                }
                if (id == 'AllPositive' || id == 'AllNegative') {
                    if (id == 'AllPositive') {
                        qty = 1;
                        incDecType = 'Inc';
                    }
                    else {
                        qty = -1;
                        incDecType = 'Dec';
                    }

                    $.ajax({
                        url: "../ShoppingCart/UpdateQuantityByQuoteId",
                        type: "POST",
                        data: { currentQuoteId: quoteID, quantity: qty },
                        datatype: 'html',
                        success: function (data) {
                            if ($('.gridscrollContent tr').length > 0 || $('.gridQuotescrollContent tr').length > 0) {

                                $(tableiD + ' tbody tr').each(function () {
                                    var itemId = $(this).find('td[id=ItemID]').html();
                                    productLinepro = $(this).find('input[type=text][id=' + itemId + ']').attr('data-productline');
                                    var currentquantity = $(this).find('input[type=text][id=' + itemId + ']').attr('data-currentvalue');
                                    if (productLinepro != 'PRO' && productLinepro != 'PUP') {
                                        if ((currentquantity > 1 && qty == -1) || (currentquantity >= 1 && qty == 1)) {
                                            UpdateCatalogQuantityItemPrice(qty, '', tableiD, incDecType, '', productLinepro);
                                        }
                                    }
                                });
                                updatePrice(data);
                                CalculatePrice();
                                if ($('#Include_Cataloging').prop('checked') == false) {
                                    $("#grid tfoot tr[id=-Catalog]").addClass("hide");
                                    $("#QuoteViewgrid  tfoot tr[id=-Catalog]").addClass("hide");
                                }
                                //else {
                                //    $("#grid tfoot tr[id='-Catalog']").removeClass("hide");
                                //    $("#QuoteViewgrid tfoot tr[id='-Catalog']").removeClass("hide");
                                //}
                            }
                            else {
                                $("#grid tr[id='-Catalog']").addClass("hide");
                            }
                        }
                    });
                }
                else {

                    qty = $('input[type=text][id=' + id + ']').attr('value');
                    if ($(this).hasClass('NumericUpDownBtns plusminusbtns')) {
                        qty--;
                        quantityValue = -1;
                        incDecType = 'Dec';
                    }
                    else {
                        qty++;
                        quantityValue = +1;
                        incDecType = 'Inc';
                    }


                    if (qty >= 1 && qty < 101) {
                        $.ajax({
                            url: "../ShoppingCart/QuantityPrice",
                            type: "POST",
                            data: { currentQuoteId: quoteID, quantity: qty, itemID: id, type: type },
                            datatype: 'html',
                            success: function (data) {
                                $('td div[id=' + id + ']').html('$' + parseFloat(data[1]).toFixed(2));
                                $('input[type=text][id=' + id + ']').val(qty);
                                $('input[type=text][id=' + id + ']').attr('value', qty);
                                $('input[type=text][id=' + id + ']').attr('data-currentvalue', qty);
                                productLinepro = $('input[type=text][id=' + id + ']').attr('data-productline');

                                TotalBooks();
                                if (productLinepro != 'PRO' && productLinepro != 'PUP') {
                                    if (qty >= 1) {
                                        UpdateCatalogQuantityItemPrice(quantityValue, '', tableiD, incDecType, '', productLinepro);
                                    }
                                }
                                CalculatePrice();
                                if ($('#Include_Cataloging').prop('checked') == false) {
                                    $("#grid tfoot tr[id=-Catalog]").addClass("hide");
                                    $("#QuoteViewgrid  tfoot tr[id=-Catalog]").addClass("hide");
                                }
                                //else {
                                //    $("#grid tfoot tr[id='-Catalog']").removeClass("hide");
                                //    $("#QuoteViewgrid tfoot tr[id='-Catalog']").removeClass("hide");
                                //}
                                $('.NumericUpDownBtns').unbind('click');
                                $("#lblTotalItemPrice").text('$' + parseFloat(data[2]).toFixed(2));
                                $("#SCItemPrice a").text('$' + parseFloat(data[2]).toFixed(2));
                                $('#SCItemCount sup').html(data[0]);
                            }

                        });
                    }
                }
            });

            function updatePrice(data) {
                var itemIDs = eval(data); // this will convert your json string to a javascript object
                for (var key in itemIDs) {
                    if (itemIDs.hasOwnProperty(key)) {
                        var itemID = key;
                        var quantityPrice = eval(itemIDs[key]);
                        for (var key1 in quantityPrice) {
                            if (quantityPrice.hasOwnProperty(key1)) {
                                var quantity = key1;
                                var price = parseFloat(quantityPrice[key1]);

                                $('div[id=' + itemID + ']').html('$' + price.toFixed(2));
                                $('input[type=text][id=' + itemID + ']').val(quantity);
                                $('input[type=text][id=' + itemID + ']').attr('value', quantity);
                                $('input[type=text][id=' + itemID + ']').attr('data-currentvalue', quantity);
                            }
                        }
                    }
                }
                TotalBooks();
                //    CalculatePrice();
            }

            function UpdateAddCartSeriesInformation(model, pageTitle, itemIdsList) {

                if (pageTitle == "Categories" || pageTitle == "Products") {
                    var itemtotalPrice = 0;
                    var itemCount = 0;
                    $.each(model.KPLItemListVM, function (i, val) {
                        itemtotalPrice = itemtotalPrice + parseFloat(val['Price']);
                        itemCount = itemCount + 1;
                    });
                    itemtotalPrice = parseFloat($("#lblTotalItemPrice").text().replace('$', '').replace(',', '')) + itemtotalPrice;
                    $("#lblTotalItemTitles").text(parseInt($("#lblTotalItemTitles").text().replace(",", "")) + itemCount);
                    $("#lblTotalItemPrice").text('$' + itemtotalPrice.toFixed(2));
                    if ($("#SCItemCount sup").text() != '0') {

                        $('#btnOrder').removeClass('disabled');
                        $('#btnOrder').removeAttr('disabled');
                    }
                    var jsnModelData1 = JSON.parse(jsonModel);
                    $.each(jsnModelData1, function (k, v) {
                        var itemPvm = v["ItemPVM"];
                        $(itemPvm["ListItemVM"]).each(function (i, item) {
                            $.each(itemIdsList, function () {
                                if (item["ItemID"] == this) {
                                    if (item["IsInSCDWQuote"] != "true") {
                                        item["IsInSCDWQuote"] = "true";
                                    }
                                }
                            });
                        });
                    });
                    if (updatedFilterjsonModel != null && updatedFilterjsonModel != undefined) {
                        $.each(updatedFilterjsonModel, function (k, v) {
                            var itemPvm = v["ItemPVM"];
                            $(itemPvm["ListItemVM"]).each(function (i, item) {
                                if (item != null) {
                                    $.each(itemIdsList, function () {
                                        if (item["ItemID"] == this) {
                                            if (item["IsInSCDWQuote"] != "true") {
                                                item["IsInSCDWQuote"] = "true";
                                            }
                                        }
                                    });
                                }
                            });
                        });
                    }
                    jsonModel = JSON.stringify(jsnModelData1);
                }
                if (pageTitle == "DecisionWizard") {
                    totalPrice = parseFloat($("#lblTotalPrice").text().replace('$', '').replace(',', ''));
                    totalPrice = DWQuoteShoppingRwInsertion(model.KPLItemListVM, totalPrice, "#grid");
                    CalculatePrice();
                    $("#lblTotalTitles").text(parseInt($("#lblTotalTitles").text()) + parseInt(model.KPLItemListVM.length));
                }
                if (pageTitle == "ShoppingCart") {
                    $("#lblTotalBooks").text(parseInt($("#lblTotalBooks").text()) + parseInt(model.KPLItemListVM.length));
                    totalPrice = DWQuoteShoppingRwInsertion(model.KPLItemListVM, totalPrice, "#grid");
                    CalculatePrice();
                    $("#lblTotalTitles").text(parseInt($("#lblTotalTitles").text()) + parseInt(model.KPLItemListVM.length));
                    if ($("#lblTotalTitles").text() != "0") {
                        $("#btnOrder").removeAttr('disabled');
                    }
                }
                if (pageTitle == "QuoteView") {
                    totalPrice = DWQuoteShoppingRwInsertion(model.KPLItemListVM, totalPrice, "#QuoteViewgrid");
                    CalculatePrice();
                }
                if (pageTitle == "KPL") {
                    $.each(model.KPLItemListVM, function (i, val) {
                        ItemPrice(val['ItemID'], "Inc");
                    });
                }


            }
            $('.NumericUpDownTxt').live('keypress', function (event) {
                var enteredqty = event.which == 13 ? this.value : this.value + String.fromCharCode(event.which);
                var isQtyValide = enteredqty >= 1 && enteredqty < 101;
                var productLinepro = '';
                var enteredValue = 0;
                if (event.which >= 48 && event.which <= 57 && isQtyValide) {
                }
                else {
                    if (event.which == 13 && isQtyValide) {
                        var previousqty = $(this).attr('data-currentvalue');
                        var updatedquantity = parseInt(enteredqty) - parseInt(previousqty);
                        var itemId = this.id;
                        $.ajax({
                            url: "../ShoppingCart/QuantityPrice",
                            type: "POST",
                            data: { currentQuoteId: quoteID, quantity: enteredqty, itemID: itemId },
                            datatype: 'html',
                            success: function (data) {
                                if (updatedquantity != 0) {
                                    $('td div[id=' + itemId + ']').html('$' + parseFloat(data[1]).toFixed(2));
                                    $('input[type=text][id=' + itemId + ']').val(enteredqty);
                                    $('input[type=text][id=' + itemId + ']').attr('data-currentvalue', enteredqty);
                                    var tblName = pageTitle == "QuoteView" ? "#QuoteViewgrid" : "#grid";
                                    productLinepro = $('input[type=text][id=' + itemId + ']').attr('data-productline');
                                    var currentquantity = $('input[type=text][id=' + itemId + ']').attr('data-currentvalue');
                                    if (productLinepro != 'PRO' && productLinepro != 'PUP') {
                                        UpdateCatalogQuantityItemPrice((parseInt(enteredqty) - parseInt(previousqty)), '', tblName, 'INC', '', productLinepro);
                                    }
                                    TotalBooks();
                                    CalculatePrice();
                                    //if ($('#Include_Cataloging').prop('checked') == false) {
                                    //    $("#grid tfoot tr[id=-Catalog]").addClass("hide");
                                    //    $("#QuoteViewgrid  tfoot tr[id=-Catalog]").addClass("hide");
                                    //}
                                    //else {
                                    //    $("#grid tfoot tr[id='-Catalog']").removeClass("hide");
                                    //    $("#QuoteViewgrid tfoot tr[id='-Catalog']").removeClass("hide");
                                    //}
                                    $("#lblTotalItemPrice").text('$' + parseFloat(data[2]).toFixed(2));
                                    $("#SCItemPrice a").text('$' + parseFloat(data[2]).toFixed(2));
                                    $('#SCItemCount sup').html(data[0]);
                                }
                            }
                        });
                        $(this).blur();
                    }
                    else {
                        event.preventDefault();
                        alert('Quantity Should be a number between 1 to 100');
                    }
                }
                //$.ajax({
                //    success: function (data) {
                //        var result = $('<table />').append(data).find('#grid').html();
                //        $('#grid').html(result);
                //    }
                //});
            });
            function DWQuoteShoppingRwInsertion(KPLItemListVM, totalPrice, divId) {
                $.each(KPLItemListVM, function (i, val) {
                    var $trrow = $("#tblrowclone tbody tr:first").clone();
                    //if (document.getElementById("QuoteViewgrid") == null) {
                    //    $trrow = $(divId + " tbody tr:first").clone();
                    //}
                    //else {

                    //    $trrow = $(divId + " tbody tr:first").clone();

                    //}
                    var productlineforselectrow = "";
                    var LevelType = [];
                    totalPrice = totalPrice + parseFloat(val['Price']);
                    $trrow.find('td').each(function () {

                        if (this.id == "Title") {
                            var imgcntrlpath = pageTitle == "ShoppingCart" ? $('#hdnImgControlPath').val() : $('#hdnImgQuoteViewControlPath').val();
                            var imgPath = imgcntrlpath + "\\" + val['ISBN'] + ".jpg";
                            $(this).find('img').attr('src', imgPath);
                            $(this).find('a').attr('Title', val['ItemID']);
                            $(this).find('a').text(val['Title'])
                        }
                        else {
                            if (this.id == 'ISBN') {
                                $(this).text('');
                                $(this).html(val['ISBN'] + "&nbsp;");
                            }

                            if (this.id == 'ItemID') {
                                $(this).text(val['ItemID']);
                            }

                            if (this.id == 'Series') {
                                $(this).text(val['Series']);
                            }

                            if (this.id == "Author") {
                                $(this).text(val['Author']);
                            }

                            if (this.id == "Lexile") {
                                $(this).text(val['Lexile']);
                                if (val['Lexile'] != '' && val['Lexile'] != null) {
                                    LevelType.push("Lexile");
                                }
                            }
                            if (this.id == "AR") {
                                $(this).attr('data-RC', val['RCLevel']);
                                $(this).text(val['ARLevel']);
                                if (val['ARLevel'] != '' && val['ARLevel'] != null) {
                                    LevelType.push("AR");
                                }
                            }
                            if (val['RCLevel'] != '' && val['RCLevel'] != null) {
                                LevelType.push("RC");
                            }

                        }
                        if (this.id == 'ItemPrice') {

                            $(this).children('div').attr('id', val['ItemID']);
                            $(this).children('div').text('$' + val['Price']);
                            if (pageTitle != "DecisionWizard") {
                                $(this).text('$' + val['Price']);
                            }

                        }
                        if (this.id == 'TtlPrice') {

                            $(this).children('div').attr('id', val['ItemID']);
                            $(this).children('div').text('$' + val['Price']);
                        }
                        if (this.id == 'Del') {
                            $(this).children('a').attr('data-Title', val['Title']);
                            $(this).children('a').attr('title', val['ItemID']);
                        }
                        if (this.id == 'calPrice') {
                            $(this).find('div').text('$' + val['Price']);
                            $(this).find('div').attr('id', val['ItemID']);
                        }
                        if (this.id == 'Quantity') {
                            $(this).children('div').children('div:first').prop('title', val['ItemID']);
                            $(this).find('input[type=text]').attr('value', '1');
                            $(this).find('input[type=text]').attr('data-currentvalue', '1');
                            $(this).find('input[type=text]').attr('id', val['ItemID']);
                            $(this).find('input[type=hidden]').attr('id', val['QuoteID'])
                            $(this).find('input[type=hidden]').prop('value', val['ItemID']);
                            $(this).children('div').find('a').prop('title', val['ItemID']);
                            $(this).find('input[type=text]').attr('data-productline', val['ProductLine']);
                            productlineforselectrow = val['ProductLine'];
                        }
                    });
                    var trid = '-' + val['ItemID'];
                    // var insertPosition = $(divId + ' tbody tr[id=-Catalog]').length != 0 ? $(divId + ' tbody tr').length - $(divId + ' tbody tr[id=-Catalog]').length - 1 : $(divId + ' tbody tr').length - 1;
                    if (document.getElementById("QuoteViewgrid") == null) {
                        $(divId + ' tbody').append("<tr id='" + trid + "' style='white-space: nowrap; overflow: hidden;'>" + $trrow.html() + "</tr>");
                    }
                    else {
                        $(divId + ' tbody').append("<tr id='" + trid + "' style='white-space: nowrap; overflow: hidden;'>" + $trrow.html() + "</tr>");
                    }


                    if ($('#Include_Cataloging').prop('checked') == false) {
                        $("#grid tfoot tr[id=-Catalog]").addClass("hide");
                        $("#QuoteViewgrid  tfoot tr[id=-Catalog]").addClass("hide");
                    }
                    else {
                        $("#grid tfoot tr[id='-Catalog']").removeClass("hide");
                        $("#QuoteViewgrid tfoot tr[id='-Catalog']").removeClass("hide");
                    }
                    UpdateCatalogQuantityItemPrice(1, '', divId, '', LevelType, productlineforselectrow);


                    $('.popup-with-zoom-anim').magnificPopup({
                        type: 'inline',
                        fixedContentPos: false,
                        fixedBgPos: true,
                        overflowY: 'auto',
                        closeBtnInside: true,
                        preloader: false,
                        midClick: true,
                        removalDelay: 300,
                        mainClass: 'my-mfp-zoom-in',
                    });
                    $('.NumericUpDownBtns').unbind('click');
                });



                return totalPrice;
            }

            function QuoteShoppingCatalogRowInsertion(catalogModel, totalPrice, divId) {

                $.each(catalogModel, function (i, val) {

                    var $trrow;
                    if (document.getElementById("QuoteViewgrid") == null) {
                        $trrow = $(divId + " tbody tr:last").clone();
                    }
                    else {

                        $trrow = $(divId + " tbody tr:last").clone();

                    }

                    totalPrice = totalPrice + parseFloat(val['Price']);
                    $trrow.find('td').each(function () {
                        if (this.id == "Title") {
                            $(this).children('a').attr('Title', val['ItemNumber']);
                            $(this).children('a').text(val['ItemNumber'])
                        }
                        else {
                            if (this.id == 'ISBN') {
                                $(this).text('');
                            }

                            if (this.id == 'Series') {
                                $(this).text('');
                            }

                            if (this.id == "Author") {
                                $(this).text('');
                            }
                        }
                        if (this.id == 'ItemPrice') {

                            $(this).children('div').attr('id', val['ItemNumber']);
                            $(this).children('div').text('$' + Math.round(val['ItemPrice'], 2));
                            $(this).text('$' + Math.round(val['Price'], 2));

                        }
                        if (this.id == 'TtlPrice') {

                            $(this).children('div').attr('id', val['ItemNumber']);
                            $(this).children('div').text('$' + Math.round(val['Price'], 2));
                        }
                        if (this.id == 'Del') {
                            $(this).children('a').attr('data-Title', val['ItemNumber']);
                            $(this).children('a').attr('title', val['ItemNumber']);
                        }
                        if (this.id == 'calPrice') {
                            $(this).find('div').text('$' + Math.round(val['ItemPrice'], 2));
                            $(this).find('div').attr('id', val['ItemNumber']);
                        }
                        if (this.id == 'Quantity') {
                            $(this).children('div').children('div:first').prop('title', val['ItemNumber']);
                            $(this).find('input[type=text]').attr('value', val['Quantity']);
                            $(this).find('input[type=text]').attr('id', val['ItemNumber']);
                            $(this).find('input[type=hidden]').attr('id', val['QuoteID'])
                            $(this).find('input[type=hidden]').prop('value', val['ItemNumber']);
                            $(this).children('div').find('a').prop('title', val['ItemNumber']);
                        }

                    });
                    var trid = '-' + val['ItemNumber'];
                    if (document.getElementById("QuoteViewgrid") == null) {
                        $(divId).last().append("<tr id='" + trid + "' style='white-space: nowrap; overflow: hidden;'>" + $trrow.html() + "</tr>");
                    }
                    else {
                        $(divId).last().append("<tr id='" + trid + "' style='white-space: nowrap; overflow: hidden;'>" + $trrow.html() + "</tr>");
                    }
                    $('.popup-with-zoom-anim').magnificPopup({
                        type: 'inline',
                        fixedContentPos: false,
                        fixedBgPos: true,
                        overflowY: 'auto',
                        closeBtnInside: true,
                        preloader: false,
                        midClick: true,
                        removalDelay: 300,
                        mainClass: 'my-mfp-zoom-in',
                    });
                    $('.NumericUpDownBtns').unbind('click');
                });



                return totalPrice;
            }
            $('#ShowImages').on("click", function () {
                var checked = $(this).is(':checked');
                if (checked == true) {
                    $('tbody img').each(function () { $(this).removeClass('hide'); });
                }
                else {
                    $('tbody img').each(function () { $(this).addClass('hide'); });
                }
            });

            $('.btnsec').live('click', function () {
                $("#btnsecondclose").removeClass("hide");
            });

            var loginPath = $('#hdnLoginPath').val();
            $('.login').live('click', function () {
                $('#loadingSetView').html('');
                $('#small-dialog').css("height", "316px");
                $('#small-dialog').css("width", "470px").css('margin-left', '42%');
                if ($.browser.mozilla || $.browser.msie) {
                    $("#loginfieldset tbody tr").find('#password').css("width", "435px");
                    $("#loadingSetView").css("margin-top", "-18px");
                    $("#loadingSetView").css("margin-left", "-4px");
                }
                $("#btnsecondclose").addClass("hide");
                $.ajax({
                    url: loginPath,
                    datatype: 'html',
                    data: { returnUrl: '' },
                    success: function (data) {
                        if (pageTitle == "ItemListView") {
                            $.magnificPopup.open({
                                items: {
                                    src: $('#loginPopup-dialog').html(),
                                    type: 'inline'
                                },
                                modal: true
                            });
                            var result = $('<div />').append(data).find('#divLoginForm').html();
                            $('#loadingLoginSetView').html(result);
                        }
                        else {
                            var result = $('<div />').append(data).find('#divLoginForm').html();
                            $('#loadingSetView').html(result);
                        }
                    }
                });
            });
            if (pageTitle != 'ShoppingCart') {
                $('#btnDeleteCart').live("click", function () {
                    var levelType = [];
                    var productLinePro = '';
                    var quantity = $('#' + itemId).val();
                    var quoteId = $("#hdnQuoteID").val();
                    $.ajax({
                        url: "../ShoppingCart/DeleteItem",
                        type: "POST",
                        data: { quoteID: quoteId, item: itemId, QuoteTypeID: 0 },
                        datatype: 'html',
                        success: function (data) {
                            levelType = GetLevelTYpe($('tr[id=-' + itemId + ']'));
                            productLinePro = $('tr[id=-' + itemId + ']').find('input[type=text][id=' + itemId + ']').attr('data-productline');
                            $('tr[id=-' + itemId + ']').remove();
                            $('.tablesorter').trigger('update');
                            if (pageViewTitle == "QuoteView") {
                                if ($('.gridQuotescrollContent tr').length > 0) {
                                    if (productLinePro != 'PRO' && productLinePro != 'PUP') {
                                        UpdateCatalogQuantityItemPrice(-(quantity), '', '#QuoteViewgrid', '', levelType, productLinePro);
                                    }
                                    var itemscount = $('#QuoteViewgrid tbody tr').size();
                                    CalculatePrice();
                                    if ($('#Include_Cataloging').prop('checked') == false) {
                                        $("#grid tr[id='-Catalog']").addClass("hide");
                                        $("#QuoteViewgrid  tr[id='-Catalog']").addClass("hide");
                                    }
                                }
                                else {
                                    $("#grid tr[id='-Catalog']").addClass("hide");
                                    $("#QuoteViewgrid tr[id='-Catalog']").addClass("hide");
                                    $('#totalPrice').html('$0.00');
                                    $('#SalesTax').html('$0.00');
                                }

                                // UpdateCatalogQuantityItemPrice(-(quantity), '', "#QuoteViewgrid");
                            }
                            else {
                                CalculatePrice();
                                if ($('#Include_Cataloging').prop('checked') == false) {
                                    $("#grid tr[id='-Catalog']").addClass("hide");
                                    $("#QuoteViewgrid  tr[id='-Catalog']").addClass("hide");
                                }
                            }

                            TotalTitles();
                            if (pageViewTitle == "QuoteView") {
                                if ($('.gridQuotescrollContent tr').length > 0) {
                                    $('#countprice').html(itemscount + ' items ' + $('#totalPrice').html());
                                }
                                else {
                                    $('#countprice').html(0 + ' items ' + $('#totalPrice').html());
                                }
                            }
                        }
                    });
                    $.magnificPopup.close();
                });
            }



            $('#btnOrder').live("click", function () {
                var poText = $('#PoText').val();
                var comments = $('#Comments').val();
                var orderQuoteId;
                $.ajax({
                    url: "../ShoppingCart/PlaceOrder",
                    type: "POST",
                    async: false,
                    data: { quoteID: quoteID, POText: poText, comments: comments },
                    datatype: 'html',
                    success: function (data) {
                        $('.gridscrollContent tr').remove();
                        $('.gridfixedFooter  tr[id=-Catalog]').remove();
                        $('#lblTotalTitles').html('0');
                        $('#lblTotalItemTitles').html('0');
                        $('#lblTotalItemPrice').html('$0.00');
                        $('#totalPrice').html('$0.00');
                        $('#lblTotalPrice').html('$0.00');
                        $('#lblTotalBooks').html('0');
                        $('#SCItemCount sup').html('0');
                        $('#SCItemPrice a').html('$0.00');
                        $('.tablesorter').trigger('update');
                        $('#SalesTax').html('$0.00');
                        $('#PoText').val('');
                        $('#Comments').val('');
                        $('#countprice').html(0 + ' items ' + $('#totalPrice').html());
                        $.magnificPopup.close();
                        orderQuoteId = data;
                        if (orderQuoteId != undefined && orderQuoteId != "") {
                            $.ajax({
                                url: "../ShoppingCart/CartDWPdfGenerationforEmail",
                                type: "POST",
                                //  async: false,
                                data: { cartDWID: orderQuoteId },
                                datatype: 'html',
                                success: function (data) {

                                }
                            });
                        }
                        // var url = '/ShoppingCart/CartDWPdfGeneration?cartDWID=' + orderQuoteId;
                        // window.open(url, "_blank");

                        $("div[id^=carttext]").addClass("hide");
                        $("div[id^=carttextlink]").removeClass("hide").show();

                        ////Commented for opening in a new window --added async:false to allow popups
                        //$('#order-pdf').html('Loading Order PDF...');
                        //$('#order-pdf').html('<embed id="PDF" width="820px" height="700px" src="' + url + '" type="application/pdf" style="margin-left:26%" />').css('margin', '0 auto 0');
                        //$.magnificPopup.open({
                        //    items: {
                        //        src: $('#order-pdfdialog').html(),
                        //        type: 'inline'
                        //    },
                        //    modal: true
                        //});

                    }
                });
                $('#pdfgeneration').attr('href', $('#pdfgeneration').attr('data-href') + "?cartDWID=" + orderQuoteId);

                // $('#pdfgeneration').click();
            });

            var pathBarcode = $('#hdnViewSingleItembyISBNPath').val();

            $('#Barcode').keyup(function (e) {
                if (e.keyCode == 13) {
                    var value = $(this).val();
                    var noWhitespaceValue = value.replace(/\s+/g, '');
                    var noWhitespaceCount = noWhitespaceValue.length;
                    if (noWhitespaceCount % 13 === 0) {
                        $.ajax({
                            url: pathBarcode,
                            type: "POST",
                            data: { ISBN: value, QuoteID: quoteID },
                            datatype: 'html',
                            success: function (data) {
                                if (data == '') {
                                    $("#Barcode").effect("shake", { times: 4 }, 100);
                                    $("#Barcode").val('');
                                }
                                else {
                                    $('.small-dialog').css("height", "640px");
                                    $('.small-dialog').css("width", "960px").css('margin', '0 auto 0');
                                    $.magnificPopup.open({
                                        items: {
                                            src: $('#barcode-dialog').html(),
                                            type: 'inline'
                                        },
                                        modal: true
                                    });
                                    $('#BarcodeloadingSetView').html('');
                                    $('#BarcodeloadingSetView').html(data);
                                    $('#Barcode').val('');
                                    $('.group2').removeClass('hide');
                                }

                            },
                            error: function (data) {
                                $("#Barcode").effect("shake", { times: 4 }, 100);
                                $("#Barcode").val('');
                            }
                        });
                    }
                    else {
                        $("#Barcode").effect("shake", { times: 4 }, 100);
                        $("#Barcode").val('');
                    }
                }
            });


        }
        //Common Script----------------------------------------------------------------------------------------------End
        function BreadCrumb() {
            var title = $("#hdnTitle").val();
            var quoteStatus = $("#hdnQuoteTypeStatus").val();
            var quoteType = $("#hdnQuoteType").val();
            var quoteTitle = $("#hdnQuoteTitle").val().trim();
            var groupName = $("#hdnGroupName").val();
            var activeQuotepath = $('#hdnActiveQuotePath').val();
            var quoteViewPath = $('#hdnViewQuoteDwPath').val();
            var indexPath = $('#hdnIndexPath').val();
            if ($("#hdnRepoStatus").val() == "True" || $("#hdnAdminRepStatus").val() == "True") {
                $('#divMenus').removeClass('hide').addClass('visible');
                $('#ulBreadcrumb').append('<li class="navlihide"><a href="' + activeQuotepath + '" >' + $("#hdnName").val() + '</a></li>');
                if (quoteStatus == "True") {
                    if (quoteType == $("#hdnShoppingCartliText").val() || quoteType == $("#htnCatalogInfoText").val() || quoteType == "Products") {

                        var viewPath = "";
                        viewPath = ((title == "Categories") && (quoteType == "Shopping Cart")) ? quoteViewPath : "";
                        $('#ulBreadcrumb').append('<li class="navlihide"><a href="' + viewPath + '">' + quoteType + '</a></li>');
                        if (viewPath != "") {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="#">' + $("#hdnFilterListText").val() + '</a></li>');
                        }
                    }
                    else {

                        if (quoteType == $("#hdnDecisionWizardliText").val()) {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="' + quoteViewPath + '">' + quoteTitle + ' ' + quoteType + '</a></li>');
                        }
                        else if (groupName == '') {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="' + quoteViewPath + '">' + quoteType + ' Quote</a></li>');
                        }
                        if (title == "KPL") {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="#">' + $("#hdnItemListText").val() + '</a></li>');
                        }
                        if (title == "Categories") {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="#">' + $("#hdnFilterListText").val() + '</a></li>');
                        }
                        if (title == "ItemListView") {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="#">' + groupName + '</a></li>');
                        }
                        if (title == "Search" || title == "Filter List") {
                            $('#ulBreadcrumb').append('<li class="navlihide"><a href="#">' + title + '</a></li>');
                        }
                    }
                }
                else {
                    if (title != "ActiveQuote") {
                        $('#ulBreadcrumb').append('<li class="navlihide"><a href="#">' + title + '</a></li>');
                    }
                }
                $('#ulBreadcrumb li:last-child a').removeAttr("href");
                $('#ulBreadcrumb li:last-child').addClass('active');
            }
            else {
                $('#divMenusCustRole').removeClass('hide').addClass('visible');
                $('#ulBreadcrumbCust').append('<li class="navlihide"><a href="' + indexPath + '">Home</a></li>');
                if (quoteStatus == "True") {
                    if (title == "ItemListView") {

                        if (quoteType == $("#hdnDecisionWizardliText").val()) {
                            $('#ulBreadcrumbCust').append('<li class="navlihide"><a>' + quoteTitle + ' ' + quoteType + '</a></li>');
                        }
                        else {
                            $('#ulBreadcrumbCust').append('<li class="navlihide"><a>' + groupName + '</a></li>');

                        }
                    }
                    else {
                        title = quoteType == "Products" ? quoteType : title;
                        $('#ulBreadcrumbCust').append('<li class="navlihide"><a href="#">' + title + '</a></li>');
                        $('#ulBreadcrumbCust li:last-child a').removeAttr("href");
                    }
                }
                else {
                    $('#ulBreadcrumbCust').append('<li class="navlihide"><a href="#">' + title + '</a></li>');
                }

                $('#ulBreadcrumbCust li:last-child').addClass('active');
            }
            $('.navlihide').show();
        }
        function GetLevelTYpe(mtrrow) {
            var levelType = [];
            if (mtrrow.find('td[id=AR]').text().trim() != '') {
                levelType.push("AR");
            }
            if (mtrrow.find('td[id=Lexile]').text().trim() != '') {
                levelType.push("Lexile");
            }
            if (mtrrow.find('td[id=AR]').attr('data-RC') != '') {
                levelType.push("RC");
            }

            return levelType;
        }
        var jsonModel = $('#hdnGroupViewModel').val();
        var updatedFilterjsonModel;
        function CategoriesViewScript() {
            var querystring = location.search.replace('?', '').split('&');
            if (querystring.length > 0) {
                if (querystring[4] != undefined) {
                    var value, fromPdfKey, fromPdf, itemId, quoteId;
                    fromPdfKey = querystring[4].split('=')[0];
                    fromPdf = querystring[4].split('=')[1];
                    quoteId = querystring[3].split('=')[1];
                    if (fromPdfKey == 'fromPdf' && fromPdf == 'True') {
                        itemId = querystring[5].split('=')[1];
                        var viewSingleItemPath = $('#hdnViewSingleItemPath').val();
                        $.ajax({
                            url: viewSingleItemPath,
                            type: "POST",
                            data: { itemID: itemId, QuoteID: 0 },
                            datatype: 'html',
                            success: function (data) {
                                $('.small-dialog').css("height", "640px");
                                $('.small-dialog').css("width", "960px").css('margin', '0 auto 0');
                                $.magnificPopup.open({
                                    items: {
                                        src: $('#barcode-dialog').html(),
                                        type: 'inline'
                                    },
                                    modal: true
                                });
                                $('#BarcodeloadingSetView').html('');
                                $('#BarcodeloadingSetView').html(data);
                                $('.popup-with-zoom-anim').magnificPopup({
                                    type: 'inline',
                                    fixedContentPos: false,
                                    fixedBgPos: true,
                                    overflowY: 'auto',
                                    closeBtnInside: true,
                                    preloader: false,
                                    midClick: true,
                                    removalDelay: 300,
                                    mainClass: 'my-mfp-zoom-in',
                                });
                            },
                            error: function (data) {
                                alert(data);
                            }
                        });
                    }
                }
            }
            var jsonupdatedModel = $('#hdnGroupViewModel').val();
            var currentDisplaytext = $('#hdncurrentDisplayText').val();
            var columnFilterText = $('#hdnColumnText').val();
            var rowFilterText = $('#hdnRowText').val();
            var slidecount = $('#hdnSlideCount').val();
            var parentchk = false;
            var chkfilter = false;
            var imagePath = $('#hdnImgPath').val();
            var pageTitle = $('#hdnPageTitle').val();
            var historyID = $('#hdnGroupPurchasingID').val();
            var pacakgeID = $('#hdnGroupPackageID').val();
            var collectionText = $('#hdnCollectionTitleText').val();
            $('#headerbartext').text(collectionText);
            var positiveFilter = false;
            //  $('.itemTextWidth').hide();
            var quoteID = $('#hdnQuoteID').val();
            //hiding the remaing column filters.
            var columnclassname = columnFilterText;
            var columnGroupList = [];
            $('.childCheckBox').unbind('click');

            $('.childCheckBox').unbind('click').live('click', function () {
                $("#Barcode").focus();
                var quoteId = $("#hdnQuoteID").val();
                var quoteType = $("#hdnQuoteType").val();
                var pkgId = $("#hdnPkgID").val();
                var title = $("#hdnTitle").val();
                var grpIds = [];
                $(".childCheckBox").each(function () {
                    if ($(this).is(':checked')) {
                        grpIds = $(this).attr('data-groupId') + '_' + grpIds;
                    }
                });
                grpIds = grpIds.slice(0, grpIds.length - 1);
                $.ajax({
                    url: '../ItemContainerPartial/GetSelectedCollectionPaginationItem',
                    data: { groupID: pkgId, currentPageIndex: 1, noofItemsPerPage: 60, selectedPackageIdsList: grpIds.toString(), quoteID: quoteId },
                    type: "POST",
                    datatype: 'html',
                    success: function (data) {
                        $('#ViewItemConatiner').html('');
                        $('#ViewItemConatiner').html(data);
                        $('.popup-with-zoom-anim').magnificPopup({

                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                        $("#Barcode").focus();
                    }
                });
            });
            $('#ddlPageCount').live('change', function () {
                $("#Barcode").focus();
                var quoteId = $("#hdnQuoteID").val();
                var pkgId = $("#hdnPkgID").val();
                var quoteType = $("#hdnQuoteType").val();
                var title = $("#hdnTitle").val();
                var grpIds = [];
                var selectedItemsPePage;
                if ($('#ddlPageCount').val() != "") {
                    if ($('#ddlPageCount').val() == "All") {
                        selectedItemsPePage = parseInt($('#hdnPageCount').val());
                    }
                    else {
                        selectedItemsPePage = parseInt($('#ddlPageCount').val());
                    }
                    $(".childCheckBox").each(function () {
                        if ($(this).is(':checked')) {
                            grpIds = $(this).attr('data-groupId') + '_' + grpIds;
                        }
                    });
                    grpIds = grpIds.slice(0, grpIds.length - 1);
                    $.ajax({
                        async: false,
                        url: '../ItemContainerPartial/GetSelectedCollectionPaginationItem',
                        data: { groupID: pkgId, currentPageIndex: 1, noofItemsPerPage: selectedItemsPePage, selectedPackageIdsList: grpIds.toString(), quoteID: quoteId },
                        type: "POST",
                        datatype: 'html',
                        success: function (data) {
                            $('#ViewItemConatiner').html('');
                            $('#ViewItemConatiner').html(data);
                            $('.popup-with-zoom-anim').magnificPopup({
                                type: 'inline',
                                fixedContentPos: false,
                                fixedBgPos: true,
                                overflowY: 'auto',
                                closeBtnInside: true,
                                preloader: false,
                                midClick: true,
                                removalDelay: 300,
                                mainClass: 'my-mfp-zoom-in',
                            });
                            $("#Barcode").focus();
                        }
                    });
                    $("#Barcode").focus();
                }
                $("#Barcode").focus();
            });

            //for cleal All Checkbox
            var clearAll = false;
            $('.clearAll').live("click", function () {
                var quoteId = $("#hdnQuoteID").val();
                var pkgId = $("#hdnPkgID").val();
                var grpIds = [];
                $(".childCheckBox").prop('checked', false);
                //$(".childCheckBox").each(function () {
                //    if ($(this).is(':checked')) {
                //        grpIds = $(this).attr('data-groupid') + '_' + grpIds;
                //    }
                //});
                //grpIds = grpIds.slice(0, grpIds.length - 1);
                $.ajax({
                    url: '../ItemContainerPartial/GetSelectedCollectionPaginationItem',
                    data: { groupID: pkgId, currentPageIndex: 1, noofItemsPerPage: 60, selectedPackageIdsList: "", quoteID: quoteId },
                    type: "POST",
                    datatype: 'html',
                    success: function (data) {
                        $('#ViewItemConatiner').html('');
                        $('#ViewItemConatiner').html(data);
                        $('.popup-with-zoom-anim').magnificPopup({
                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                    }
                });
            });


            $('.' + columnclassname).each(function () {
                var selectedFilterGroupId = $(this).attr('data-SubGrpId');
                $('.packageGroup').each(function () {
                    var packageGrpId = $(this).attr('data-groupid');
                    if (packageGrpId == selectedFilterGroupId) {
                        $(this).hide();
                        return true;
                    }
                });
            });

            //For All Checkbox.
            $('.all').on("click", function () {
                var checked = $(this).is(':checked');
                if (checked == true) {
                    $('.filterGroup').prop('checked', true);
                    $('.packagegrp-53').show();
                    $('.packageGroup').slice(1).show();
                }
                else {
                    $('.filterGroup').prop('checked', false);
                    $('.packagegrp-53').hide();
                    $('.packageGroup').slice(1).hide();
                }
            });
            //for cleal All Checkbox
            var clearAll = false;
            //$('.clearAll').on("click", function () {
            //    var parentchk = false;
            //    var jsonupdatedModel = JSON.parse($('#hdnGroupViewModel').val());
            //    var chkfilter = false;
            //    positiveFilter = false;
            //    $('.filterGroup').attr('checked', false);
            //    $('.packageGroup').each(function () {
            //        $(this).hide();
            //    });
            //    clearAll = true;
            //    LoadingFilterData(jsonupdatedModel);
            //});

            //getting row filter GroupIDs
            var rowfilterIDS = [];
            var rowclassName = rowFilterText;
            $('.' + rowclassName).each(function () {
                rowfilterIDS.push($(this).attr('data-subGrpId'))
            });

            //hiding the packages and purchasing history filters
            $('.' + rowclassName + '-' + historyID).hide();
            $('.' + rowclassName + '-' + pacakgeID).hide();

            //pushing the checked row filter groupIDs in Array.
            var chekedrowItems = [];
            $('.' + rowclassName + ':checked').each(function () {
                chekedrowItems.push($(this).attr('data-subGrpId'))
            });

            //hiding the column filters div-slides and showing only the row filters div-slides.
            $('.' + rowclassName).each(function () {
                var allChecked = false;
                var selectedFilterGroupId = $(this).attr('data-SubGrpId');
                var parentliid = $(this).attr('id').split('-')[1];
                $('.packageGroup').hide();
                $('.packagegrp-53').show();
                $.each(chekedrowItems, function (i, val) {
                    var packageGrpId = val;
                    $('.subChildGroup-' + parentliid).each(function () {
                        if ($(this).is(':checked')) {
                            allChecked = true;
                        }
                    });
                    $('.packagegrp-' + packageGrpId).show();

                });
            });

            //Works whem parent checkbox is clicked for every group.
            $('[id^=chkParentGroup]').on('click', function (e) {
                var checked = $(this).is(':checked');
                if (checked == true) {
                    $(this).closest('li').find('input').attr('checked', true);
                }
                else {
                    $(this).closest('li').find('input').attr('checked', false);
                }

                if ($(this).attr('data-grpId') == historyID || $(this).attr('data-grpId') == pacakgeID) {
                    HidingandShowingthePackages(this);
                }
                else {
                    parentchk = true;
                    positiveFilter = false;
                    chkfilter = true;
                    CheckandUncheckFilter(this);
                }

            });


            //this function will filter the items according to the selection of checked filter.
            $('.childCheckBox').unbind('click').bind('click', function () {
                $('.carouselRight').addClass('hide');
                $('.carouselLeft').addClass('hide');
                chkfilter = true;
                var index = $(this).attr('data-grpId');
                if ($(this).attr('data-grpId') == historyID || $(this).attr('data-grpId') == pacakgeID) {
                    HidingandShowingthePackages(this);
                }
                else {
                    positiveFilter = false;
                    CheckandUncheckFilter(this);
                }
            });


            $('#lstPostiveFilters').live("change", function (e) {
                if ($('#lstPostiveFilters').val() != "") {
                    chkfilter = false;
                    positiveFilter = true;
                    $('.childCheckBox').attr('checked', 'checked');
                    $('[id^=chkParentGroup]').attr('checked', 'checked');
                    CheckandUncheckFilter(this);
                }
            });

            //function HidingandShowingthePositiveFilters(value) {
            //    var selectedFilterGroupId = value;
            //    

            //    if (selectedFilterGroupId != "") {
            //        $('.packageGroup').each(function () {
            //            var packageGrpId = $(this).attr('data-groupid');

            //            if (packageGrpId == selectedFilterGroupId) {
            //                alert(packageGrpId);
            //                $('.packagegrp-' + packageGrpId).show();
            //            }
            //        });
            //    }
            //}

            function CheckandUncheckFilter(control) {
                var chk;
                if (chkfilter == true) {
                    var allChecked = true;
                    chk = $(control).is(':checked');
                    var parentliid = $(control).attr('id').split('-')[1];
                    if (chk == true) {
                        $('.subChildGroup-' + parentliid).each(function () {
                            if ($(this).is(':not(:checked)')) {
                                allChecked = false;
                            }
                        });
                        $(control).closest('li').find('input').attr('checked', true);
                        if (allChecked) {
                            $('#chkParentGroup-' + parentliid).attr('checked', true);
                        }
                    }
                    else {
                        $(control).closest('li').find('input').attr('checked', false);
                        $(control).attr('checked', false)
                        $('#chkParentGroup-' + parentliid).attr('checked', false);
                    }
                }

                var classname = columnclassname;
                var columnfilterGroupList = [];
                if (chkfilter == true) {
                    $('.' + classname + ':not(:checked)').each(function () {
                        columnfilterGroupList.push($(this).attr('data-subGrpId'));
                    });
                }
                else {
                    if ($('#lstPostiveFilters').val() == "0") {
                        //$('#lstPostiveFilters option').each(function () {
                        //    columnfilterGroupList.push($(this).attr('value'));

                        //});
                        $('.' + rowFilterText).each(function () {
                            columnfilterGroupList.push($(this).attr('data-subGrpId'));
                        });
                    }
                    else {
                        columnfilterGroupList.push($('#lstPostiveFilters').val());
                    }

                }

                var itemIDList = [];
                var jsonGroupModel = positiveFilter == true && chkfilter == true ? updatedFilterjsonModel : JSON.parse(jsonModel);
                $.each(columnfilterGroupList, function (g, groupID) {
                    $.each(jsonGroupModel, function (k, v) {
                        if (v["lstChildItemGVM"] != null) {
                            $(v["lstChildItemGVM"]).each(function (cg, itemg) {
                                if (itemg["GroupID"] == groupID) {
                                    var itemPvm = itemg["ItemPVM"];
                                    $(itemPvm["ListItemVM"]).each(function (i, item) {
                                        if (item != null) {
                                            var itemid = item["ItemID"];
                                            itemIDList.push(itemid);
                                        }

                                    });
                                }
                            });
                        }
                        else {
                            if (v["GroupID"] == groupID) {
                                var itemPvm = v["ItemPVM"];
                                $(itemPvm["ListItemVM"]).each(function (i, item) {
                                    if (item != null) {
                                        var itemid = item["ItemID"];
                                        itemIDList.push(itemid);
                                    }
                                });
                            }
                        }
                    });
                });


                itemIDList = $.unique(itemIDList);
                var groupItemIDList = [];

                //Matching the Items with the JSON model and creating New Model With New ItemIDS
                $.each(jsonGroupModel, function (k, v) {
                    var itemPvm = v["ItemPVM"];
                    var groupID = v["GroupID"];
                    var groupItemCount = $(itemPvm["ListItemVM"]).length;
                    //if (groupID != 0) {
                    $(itemPvm["ListItemVM"]).each(function (i, item) {
                        if (item != null) {
                            var itemvalue = item["ItemID"];
                            if (positiveFilter == true && chkfilter == false) {
                                if ($.inArray(itemvalue, itemIDList) < 0) {
                                    itemPvm["ListItemVM"][i] = null;
                                }
                                else {
                                    groupItemIDList.push(itemvalue);
                                }
                            }
                            else {
                                if ($.inArray(itemvalue, itemIDList) > -1) {
                                    itemPvm["ListItemVM"][i] = null;
                                    groupItemIDList.push(itemvalue);
                                }
                            }
                        }
                    });
                    //}
                    v["GroupItemCount"] = positiveFilter == false ? parseInt(groupItemCount - groupItemIDList.length) : parseInt(groupItemIDList.length);
                    jsonupdatedModel = jsonGroupModel;
                    groupItemIDList = [];
                });

                if (parentchk == true) {
                    jsonupdatedModel = jsonGroupModel;
                }
                updatedFilterjsonModel = jsonupdatedModel;
                $('.carouselRight').addClass('hide');
                $('.carouselLeft').addClass('hide');
                if (igrploop.length <= 0) {
                    $.each(jsonupdatedModel, function (k, v) {
                        igrploop.push(0);
                    });
                }
                else {
                    $.each(jsonupdatedModel, function (k, v) {
                        igrploop[k] = 0;
                    });
                }
                LoadingFilterData(jsonupdatedModel);
            }

            function HidingandShowingthePackages(control) {
                var allChecked = true;
                var checked = $(control).is(':checked');
                var parentliid = $(control).attr('id').split('-')[1];
                $(control).closest('li').find('input').each(function () {
                    var checkcontrol = $(this);
                    var selectedFilterGroupId = $(this).attr('data-SubGrpId') == undefined ? "" : $(this).attr('data-SubGrpId')
                    if (selectedFilterGroupId != "") {
                        $('.packageGroup').each(function () {
                            var packageGrpId = $(this).attr('data-groupid');
                            if (packageGrpId == selectedFilterGroupId) {
                                if (checked == true) {
                                    $(this).show();
                                    checkcontrol.attr('checked', true);
                                    $('.subChildGroup-' + parentliid).each(function () {
                                        if ($(this).is(':not(:checked)')) {
                                            allChecked = false;
                                        }
                                    });
                                    if (allChecked) {
                                        $('#chkParentGroup-' + parentliid).attr('checked', true);
                                    }
                                    else {
                                        $('#chkParentGroup-' + parentliid).attr('checked', false);
                                    }
                                    loadingGrpItemCount(selectedFilterGroupId)
                                    return true;
                                }
                                else {
                                    $(this).hide();
                                    checkcontrol.attr('checked', false);
                                    $('#chkParentGroup-' + parentliid).attr('checked', false);
                                    return true;

                                }
                            }

                        });
                    }
                });
            }

            function loadingGrpItemCount(selectedgroupID) {
                var jsonGroupModel = JSON.parse(jsonModel);
                if (igrploop.length <= 0) {
                    $.each(jsonupdatedModel, function (k, v) {
                        igrploop.push(0);
                    });
                }
                else {
                    $.each(jsonupdatedModel, function (k, v) {
                        igrploop[k] = 0;
                    });
                }
                $.each(jsonGroupModel, function (k, v) {
                    var groupID = v["GroupID"];
                    var grpItmCount = v["GroupItemCount"]
                    if (selectedgroupID == groupID) {
                        $('#slide-' + groupID).find('.imgstar').remove();
                        $('#GrpItemCount-' + selectedgroupID).html(grpItmCount);
                    }

                });
            }


            function LoadingFilterData(filterModel) {
                var groupID;
                var className = rowFilterText;
                var rowGrpIDList = [];
                rowGrpIDList.push(0);
                if (clearAll == true) {
                    $('.' + className + ':checked').each(function () {
                        rowGrpIDList.push($(this).attr('data-subGrpId'));
                    });
                }
                else {
                    $('.' + className).each(function () {
                        rowGrpIDList.push($(this).attr('data-subGrpId'));
                    });
                }
                var lastImgID = '';
                $.each(rowGrpIDList, function (g, groupID) {
                    $.each(filterModel, function (k, v) {
                        if (v["GroupID"] == groupID) {
                            var itemPvm = v["ItemPVM"];
                            $('#GrpItemCount-' + groupID).html('');
                            var grpItmCount = v["GroupItemCount"]
                            $('#GrpItemCount-' + groupID).html(grpItmCount);
                            if (parseInt(grpItmCount) == 0) {
                                $('.packagegrp-' + groupID).hide();;
                            }
                            else {
                                $('.packagegrp-' + groupID).show();
                            }
                            var totalitemLength = itemPvm["ListItemVM"].length;
                            var igrploop = 0;
                            $('#slide-' + groupID).find('.imgstar').remove();
                            $(itemPvm["ListItemVM"]).each(function (i, item) {

                                if (item != null) {
                                    $('.carouselRight').each(function () { if (this.id == groupID) { $(this).removeClass('hide'); } });
                                    var remainderCount = (totalitemLength) % slidecount;
                                    remainderCount = parseInt(remainderCount) == 0 ? Math.ceil((totalitemLength) / slidecount) : remainderCount;
                                    var displayCount = slidecount;
                                    var totalcount = i + parseInt(displayCount) + 1
                                    if (parseInt(totalcount) > parseInt(totalitemLength)) {
                                        displayCount = remainderCount;
                                        $('.carouselRight').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });
                                    }
                                    if (parseInt(grpItmCount) <= parseInt(slidecount)) {
                                        $('.carouselRight').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });
                                    }

                                    if (className == rowFilterText) {
                                        if (i + 1 != totalitemLength) {
                                            var k = 0;
                                            var l = 0;
                                            $('.divImg-' + groupID).children('a').html('');
                                            if (authenticationStatus == "True") {
                                                $('.divCartStatus-' + groupID).html('');
                                            }
                                            else {
                                                $('.divCartStatus-' + groupID).children('a').html('');
                                            }
                                            $('.divtitle-' + groupID).children('a').html('');
                                            $('.divStarIcon-' + groupID).html('');
                                            for (var j = i; j <= i + displayCount; j++) {
                                                var jsonrow = null;
                                                if (itemPvm["ListItemVM"][j] != null) {
                                                    jsonrow = $(itemPvm["ListItemVM"]).get(j);
                                                }
                                                else {
                                                    l = j;
                                                    while (jsonrow == null && l <= totalitemLength) {
                                                        l++;
                                                        jsonrow = $(itemPvm["ListItemVM"]).get(l);
                                                        j = l;
                                                        i = l;
                                                    }
                                                    if (jsonrow != null || jsonrow != undefined) { displayCount = displayCount - 1; }
                                                }
                                                if (jsonrow != undefined) {
                                                    AddSlideImg(jsonrow, groupID, k);
                                                    k++;

                                                }

                                            }
                                            i = totalitemLength;
                                        }
                                    }

                                }

                                if (i == totalitemLength) {
                                    return (false);
                                }
                            });

                        }
                    });
                });
                $('.popup-with-zoom-anim').magnificPopup({
                    type: 'inline',
                    fixedContentPos: false,
                    fixedBgPos: true,
                    overflowY: 'auto',
                    closeBtnInside: true,
                    preloader: false,
                    midClick: true,
                    removalDelay: 300,
                    mainClass: 'my-mfp-zoom-in',
                });
            }

            var filtergrpID = 0;
            var igrploop = new Array();
            //$('.carouselLeft').attr('disabled', 'disabled');
            $('.carouselClick').unbind('click').bind("click", function (e) {

                var className = this.className;
                var carouselModel = JSON.parse(jsonModel);

                if (parseInt(filtergrpID) == 0) {
                    filtergrpID = this.id;
                }
                var groupID = this.id;
                $('#slide-' + groupID).find('.imgstar').remove();
                if (chkfilter == true || positiveFilter == true) {
                    carouselModel = jsonupdatedModel;
                }
                if (igrploop.length <= 0) {
                    $.each(carouselModel, function (k, v) {
                        igrploop.push(0);
                    });
                }
                $.each(carouselModel, function (k, v) {
                    if (v["GroupID"] == groupID) {
                        var igrpIndex = parseInt(k);
                        var grpItmCount = v["GroupItemCount"];
                        $('#GrpItemCount-' + groupID).html(grpItmCount);
                        var itemPvm = v["ItemPVM"];
                        var imgCount = $('#slide-' + groupID + ' img').length;
                        if (className == 'slideBtnShadow carouselClick carouselRight') {
                            var lastImgId = $('#slide-' + groupID + ' img')[imgCount - 1].id;
                        }
                        else {
                            var lastImgId = $('#slide-' + groupID + ' img')[0].id;
                        }

                        var totalitemLength = itemPvm["ListItemVM"].length;
                        $(itemPvm["ListItemVM"]).each(function (i, item) {
                            if (item != null && item["ItemID"] == lastImgId) {
                                var remainderCount = (totalitemLength) % slidecount;
                                var displayCount = slidecount;
                                var totalCount = parseInt(i + parseInt(displayCount) + 1)
                                //if (chkfilter == true || positiveFilter == true) {
                                //    if (parseInt(igrploop) >= parseInt(grpremainderCount)) {
                                //        $('.carouselRight').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });
                                //        filtergrpID == 0;
                                //        igrploop = 0;
                                //    }
                                //}
                                //else {
                                //    if ((parseInt(totalCount)) > parseInt(totalitemLength)) {
                                //        displayCount = remainderCount;
                                //        $('.carouselRight').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });
                                //    }
                                //}
                                if (className == 'slideBtnShadow carouselClick carouselRight') {
                                    igrploop[igrpIndex]++;

                                }
                                else {
                                    igrploop[igrpIndex]--;
                                }
                                var grpremainderCount = Math.ceil(grpItmCount / parseInt(slidecount));
                                if (parseInt(igrploop[igrpIndex]) >= parseInt(grpremainderCount) - 1) {
                                    if (chkfilter == true || positiveFilter == true) {
                                        if (parseInt(igrploop[igrpIndex]) >= parseInt(grpremainderCount)) {
                                            filtergrpID == 0;
                                        }
                                    }
                                    else {
                                        if ((parseInt(totalCount)) > parseInt(totalitemLength)) {
                                            displayCount = remainderCount;
                                        }
                                    }
                                    $('.carouselRight').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });

                                }
                                else {
                                    if (igrploop[igrpIndex] == parseInt(0)) {
                                        $('.carouselLeft').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });
                                    }
                                } if (className == 'slideBtnShadow carouselClick carouselRight') {
                                    $('.carouselLeft').each(function () { if (this.id == groupID) { $(this).removeClass('hide'); } });
                                    if (i + 1 != totalitemLength) {
                                        var k = 0;
                                        var l = 0;
                                        $('.divImg-' + groupID).children('a').html('');
                                        $('.divtitle-' + groupID).children('a').html('');
                                        $('.divStarIcon-' + groupID).html('');
                                        if (authenticationStatus == "True") {
                                            $('.divCartStatus-' + groupID).html('');
                                        }
                                        else {
                                            $('.divCartStatus-' + groupID).children('a').html('');
                                        }
                                        for (var j = i + 1 ; j <= parseInt(i) + parseInt(displayCount) ; j++) {
                                            var jsonrow = null;
                                            if (itemPvm["ListItemVM"][j] != null) {
                                                jsonrow = $(itemPvm["ListItemVM"]).get(j);
                                            }
                                            else {
                                                l = j;

                                                while (jsonrow == null && l <= totalitemLength) {
                                                    l++;
                                                    jsonrow = $(itemPvm["ListItemVM"]).get(l);
                                                    j = l;
                                                    i = l;
                                                }
                                                if (jsonrow != null || jsonrow != undefined) { displayCount = displayCount - 1; }
                                            }

                                            if (jsonrow != undefined) {
                                                AddSlideImg(jsonrow, groupID, k);
                                                k++;
                                            }
                                        }
                                        $("#slide-" + groupID).hide(0).delay(0).toggle('slide', {
                                            direction: 'right'
                                        }, 1000);
                                    }
                                }
                                else {
                                    $('.carouselRight').each(function () { if (this.id == groupID) { $(this).removeClass('hide') } });
                                    //updating by chandana
                                    var jsonrowItemID;
                                    var m = 0;
                                    var jsonItemID;
                                    jsonItemID = itemPvm["ListItemVM"][0];
                                    while (jsonItemID == null && m <= totalitemLength) {
                                        jsonItemID = $(itemPvm["ListItemVM"]).get(m);
                                        m++;
                                    }
                                    jsonrowItemID = jsonItemID["ItemID"];

                                    if (lastImgId != jsonrowItemID) {
                                        if (i != jsonrowItemID) {
                                            $("#slide-" + groupID).hide(0).delay(0).toggle('slide', {
                                                direction: 'left'
                                            }, 1000);
                                            var l = 0;
                                            var k = slidecount;
                                            $('.divImg-' + groupID).children('a').html('');
                                            $('#divtitle-' + groupID + '-' + k).children('a').html('');
                                            if (authenticationStatus == "True") {
                                                $('.divCartStatus-' + groupID).html('');
                                            }
                                            else {
                                                $('.divCartStatus-' + groupID).children('a').html('');
                                            }
                                            for (var j = i; j <= parseInt(i) + parseInt(displayCount) ; j--) {
                                                var jsonrow = null;
                                                if (itemPvm["ListItemVM"][j] != null) {
                                                    jsonrow = $(itemPvm["ListItemVM"]).get(j);
                                                }
                                                else {
                                                    l = j;
                                                    while (jsonrow == null && l <= totalitemLength) {
                                                        l--;
                                                        jsonrow = $(itemPvm["ListItemVM"]).get(l);
                                                        j = l;
                                                        i = l;
                                                    }
                                                    if (jsonrow != null || jsonrow != undefined) { displayCount = displayCount - 1; }
                                                }


                                                if (jsonrow != undefined) {
                                                    AddSlideImg(jsonrow, groupID, k);
                                                    k--;
                                                }

                                                if (k == -1) {
                                                    i == totalitemLength;
                                                    return (false);
                                                }
                                            }
                                            $("#slide-" + groupID).hide(0).delay(0).toggle('slide', {
                                                direction: 'left'
                                            }, 2000);

                                        }

                                    }
                                    else {
                                        $('.carouselLeft').each(function () { if (this.id == groupID) { $(this).addClass('hide'); } });
                                    }
                                }
                            }
                        });
                    }
                });
                $('.popup-with-zoom-anim').magnificPopup({
                    type: 'inline',
                    fixedContentPos: false,
                    fixedBgPos: true,
                    overflowY: 'auto',
                    closeBtnInside: true,
                    preloader: false,
                    midClick: true,
                    removalDelay: 300,
                    mainClass: 'my-mfp-zoom-in',
                });
            });

            function AddSlideImg(jsonrow, groupID, k) {
                var isbn = jsonrow["ISBN"] == null ? "" : jsonrow["ISBN"];
                var imgNewId = jsonrow["ItemID"];
                var title = jsonrow["Title"];
                var isInSCDWQuote = jsonrow["IsInSCDWQuote"];
                var scdwquoteText = currentDisplaytext;
                var imgcntrlpath = $('#hdnImgControlPath').val();
                var imgPath = imgcntrlpath + "\\" + isbn + ".jpg";
                var isinTitleBefore = jsonrow["IsInCustomerTitles"];
                var isPreviewItem = jsonrow["IsPreviewItem"];
                var charecterBroughtBefore = jsonrow["CharecterBroughtBefore"];
                var seriersBroughtBefore = jsonrow["SeriesBroughtBefore"];
                var scDetails = "";
                if (authenticationStatus == "True") {
                    if (seriersBroughtBefore == true && charecterBroughtBefore == false) {
                        scDetails = ' <i class="icon-star-3 fg-yellow itemTextSize pos-abs imgstar" style="margin-left:-10px"></i>';
                    }
                    if (charecterBroughtBefore == true && seriersBroughtBefore == false) {
                        scDetails = ' <i class="icon-star-3 button-fg pos-abs itemTextSize imgstar" style="margin-left:-10px"></i>';
                    }
                    //if (seriersBroughtBefore == true && charecterBroughtBefore == true) {
                    //    scDetails = ' <img src=' + $("#hdnyellowBlueStarPath").val() + ' class="wd15 pos-abs imgstar" style="width:16px;margin-left:-10px" />';
                    //}
                }
                if (pageTitle == 'HomePage') {
                    var imgHtml = '<img id="' + imgNewId + '" class="imgTest-' + groupID + '-' + k + ' titleShadow" src="' + imgPath + '">';
                    //$('#divImg-' + groupID + '-' + k).attr('style', 'height:145px');
                }
                else {
                    var imgHtml = '<img id="' + imgNewId + '" class="imgTest-' + groupID + '-' + k + ' shadow" src="' + imgPath + '" >';
                }
                $('#ViewDetailItem-' + groupID + '-' + k).removeClass('div-bgopacity');
                $('#ViewDetailItem-' + groupID + '-' + k).attr('data-Title', isinTitleBefore);
                if (isinTitleBefore) {
                    $('#ViewDetailItem-' + groupID + '-' + k).addClass('div-bgopacity');
                }
                if (isPreviewItem == false) {
                    $('#ViewDetailItem-' + groupID + '-' + k).attr('style', 'pointer-events:none;color:whitesmoke;');
                }
                $('#divtitle-' + groupID + '-' + k).children('a').attr('title', imgNewId);
                var formatedTitle = title.toString();
                if (formatedTitle.length > 19) {

                    formatedTitle = title.toString().length > 11 ? title.toString().substring(0, 11) + '\n' + title.toString().substring(11, title.toString().length) : title.toString();
                    formatedTitle = formatedTitle.length > 17 ? formatedTitle.substr(0, 17) + "..." : formatedTitle;

                    //formatedTitle = title.toString().substring(0, 16) + '\n' + title.toString().substring(18, 38) + (title.toString().length > 38 ? "..." : "");
                }
                $('#divImg-' + groupID + '-' + k).html(scDetails + $('#divImg-' + groupID + '-' + k).html());
                //$('#divtitle-' + groupID + '-' + k).children('a').html(formatedTitle); -- Commented because title is not required
                $('#divtitle-' + groupID + '-' + k).children('a').html('');
                $('#divImg-' + groupID + '-' + k).children('a').attr('title', imgNewId);
                $('#divImg-' + groupID + '-' + k).children('a').html(imgHtml);
                // $('#divStarIcon-' + groupID + '-' + k).html(scDetails);
                if (authenticationStatus == "True") {
                    if (isInSCDWQuote == false) {
                        var addToCart = '<div id="carttext-' + imgNewId + '" class="hide">In ' + scdwquoteText + '</div><div id="carttextlink-' + imgNewId + '" ><button id="1" name="1" value="1" data-itemid="' + imgNewId + '" class="AddCart" style="color:white;">Add To ' + scdwquoteText + '</button></div>';
                    }

                    else {
                        var addToCart = ' <div id="carttext-' + imgNewId + '" >In ' + scdwquoteText + '</div><div id="carttextlink-' + imgNewId + '" class="hide"><button id="1" name="1" value="1" data-itemid="' + imgNewId + '" class="AddCart" style=color:white;">Add To ' + scdwquoteText + '</button></div>';
                    }
                    $('#divCartStatus-' + groupID + '-' + k).html(addToCart);
                }
                else {
                    var addtoCart = 'Add to Cart';
                    $('#divCartStatus-' + groupID + '-' + k).children('a').html(addtoCart);
                }
                $('.AddCart').unbind('click');
                // $('.itemTextWidth').hide();
                $('.popup-with-zoom-anim').magnificPopup({
                    type: 'inline',
                    fixedContentPos: false,
                    fixedBgPos: true,
                    overflowY: 'auto',
                    closeBtnInside: true,
                    preloader: false,
                    midClick: true,
                    removalDelay: 300,
                    mainClass: 'my-mfp-zoom-in',
                });
            }

            $('.gotoPage,.collectionText').live("click", function (e) {
                $('#Loading').removeClass('hide');
            });
            $('#Loading').addClass('hide');
        }

        //Quote View Script -----------------------------------------------------------------------------------------Start
        function quoteViewScript() {
            $("#Barcode").focus();
            var quoteID = $('#hdnQuoteID').val();
            var quoteType = $('#hdnquoteType').val();
            TotalBooks();
            $('#QuoteViewgrid').tablesorter();
            var hdnIncludeCatalogStatus = $('#hdnIncludeCatalogStatus').val();
            if (hdnIncludeCatalogStatus == 'True') {
                $('.includeCatalog ').attr('checked', 'checked')
                includeCatalog = true;
            }

            $('#QuoteViewgrid tfoot>tr').each(function () {
                if ($(this).attr('id') == "-Catalog") {
                    var catalogStatus = $(this).attr('data-catalog');
                    $(this).children('th[id=Quantity]').children('div').find('div[id=Quantity]').find('input[type=text][id=Catalog]').attr('data-Catalog', catalogStatus);
                }
                if ($(this).attr('id') == "-Catalog" && hdnIncludeCatalogStatus == 'False') {
                    $(this).addClass('hide');
                }
            });
            $('.SubmitQuote').on("click", function () {
                //SubmitQuotePartialView
                SubmitQuoteFunctionality();
            });
            //$('.Quantity').change(function () {
            //    var qty = this.value;
            //    var qdID = this.title;
            //    var itmidVal = this.id;  //document.getElementById(qdID).value;
            //    $.ajax({
            //        url: "../QuoteView/QuantityPrice",
            //        type: "POST",
            //        data: { QuoteDetailID: qdID, quantity: qty, itemID: itmidVal, quoteid: quoteID },
            //        datatype: 'html',
            //        success: function (data) {
            //            var result = $('<table />').append(data).find('#QuoteViewgrid').html();
            //            $('#QuoteViewgrid').html(result);
            //        },
            //    });
            //});


            function CalculatePrice() {
                var totalPrice = 0;
                $('.calculatePrice').each(function () {
                    if ($(this).parent('th').parent('tr').attr('id') != "-Catalog" && includeCatalog == false) {
                        var itemPrice = $(this).html().replace('$', '').replace(',', '');
                        totalPrice = parseFloat(itemPrice) + parseFloat(totalPrice);
                    }
                    else if (includeCatalog == true) {
                        var itemPrice = $(this).html().replace('$', '').replace(',', '');
                        totalPrice = parseFloat(itemPrice) + parseFloat(totalPrice);
                    }
                });
                $('#totalPrice').html('$' + totalPrice.toFixed(2));
            }
            //$('.NumericUpDownTxt').keypress(function (event) {
            //    var qty = this.value;
            //    var isQtyValide=qty >= 1 && qty < 101;
            //    if (event.which >= 48 && event.which <= 57 && isQtyValide) {
            //    }
            //    else {
            //        if (event.which==13&&isQtyValide) {
            //            $.ajax({
            //                url: "../ShoppingCart/QuantityPrice",
            //                type: "POST",
            //                data: { currentQuoteId: quoteID, quantity: qty, itemID: itmidVal },
            //                datatype: 'html',
            //                success: function (data) {
            //                    $('div[id=' + id + ']').html('$' + parseFloat(data).toFixed(2));
            //                    $('input[type=text][id=' + id + ']').val(qty);
            //                    UpdateCatalogQuantityItemPrice(quantityValue, '', '#QuoteViewgrid');
            //                    CalculatePrice();
            //                }
            //            });
            //        }
            //        else {
            //            event.preventDefault();
            //            alert('Quantity Should be a number between 1 to 100');
            //        }
            //    }
            //    //$.ajax({
            //    //    success: function (data) {
            //    //        var result = $('<table />').append(data).find('#grid').html();
            //    //        $('#grid').html(result);
            //    //    }
            //    //});
            //});

            $('#QuoteTypes').val($('#QuoteTypes option:contains(' + quoteType + ')').val());



        }
        //Quote View Script------------------------------------------------------------------------------------------End
        function SingleItemView(itemid, titlename) {
            $('.itemDetails').hide();
            $('table.' + itemid + '').show();
            if ($('#ulBreadcrumbCust li').length <= 2) {
                var urltogo;
                $('#ulBreadcrumbCust').append('<li id="' + itemid + '" class="navlihide"><a href="#">' + titlename + '</a></li>');
                $('#ulBreadcrumbCust li:nth-child(2) a').prop('href', '');
                $('#ulBreadcrumbCust li:nth-child(2) a').addClass("nav-page");
                $('#ulBreadcrumbCust li:last-child a').removeAttr("href");
                $('#ulBreadcrumbCust li:last-child').addClass('active');
            }

            $('#tblStatus').addClass('div-disable');
            // $('#tb1').hide();
            $('#gotoPageDiv').hide();
            $('#divBottomList').hide();
            $('#pb1').hide();
        }
        function itemListViewScript() {
            var yesCount = $('#hdnYesCount').val();
            var noCount = $('#hdnNoCount').val();
            var mayBeCount = $('#hdnMayBeCount').val();
            var newCount = $('#hdnNewCount').val();

            calculateProgressBar();
            // $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');

            // $('#HRMenus').addClass('hide');
            yesNoMaybeStatus();
            //All checkbox is checked while loading page
            function yesNoMaybeStatus() {
                if ($('input.case').is(':checked')) {
                } else {
                    if ($('#btnviewOne').length) {
                        $('.SelectAll').attr('checked', true);
                    }
                    else {
                        $('.case').each(function () {
                            if (this.value == 5) {
                                $(this).attr('checked', true);
                            }
                        });
                    }
                }
            }
            var quoteid = $('#hdnQuoteID').val();
            var groupId = $('#hdnGroupID').val();
            var quoteType = $('#hdnQuoteType').val();
            var groupViewData = $('#hdnViewDataGroupID').val();
            var insideImagePath = $('#hdnInsideImagePath').val();
            var pageNoForSigleItem = 2;

            var groupIdsList = [];
            var title;
            var shoppingCartText = $('#hdnShoppingCartText').val();
            var categoryText = $('#hdnCategoryText').val();
            var gotoPageNo;
            var currentPageIndex = $('#hdnCurrentPageIndex').val();
            var selectionCount = $('#hdnSelectionCount').val();
            var productsText = $('#hdnProductsText').val();
            var iscasecheck = false;
            var noofpagesOnReturncall;


            $(".SelectAll").live("click", function () {
                $('#Loading').removeClass('hide');
                groupIdsList = [];
                $(".case").each(function () {
                    groupIdsList.push(this.value);
                });

                $(".case").removeAttr("checked");

                var e;
                var noitems;
                isSingleView = false;
                if ($('#btnviewOne').length > 0) {
                    // e = document.getElementById("ddlPageDenomination");
                    noitems = 0;
                }
                else {
                    noitems = 10;
                    isSingleView = true;
                    pageNoForSigleItem = 2;
                }
                //var e = document.getElementById("ddlPageDenomination");
                var urltogo = "../ItemListView/GetItemListPartial";
                var data1 = { iD: quoteid, type: quoteType, ddlSelectedValue: noitems, pgno: "1", selectionStatus: groupIdsList.toString(), IsSingleView: isSingleView };

                $.ajax({
                    url: urltogo,
                    type: "POST",
                    data: data1,
                    datatype: 'html',
                    success: function (data) {
                        $('#divMain').html('');
                        $('#divMain').html(data);
                        $(".SelectAll").attr('checked', true);
                        calculateProgressBar();
                        $('.uncheck').addClass('hide');
                        $('#Loading').addClass('hide');
                    }
                });
            });

            // if all checkbox are selected, check the selectall checkbox
            // and viceversa
            $(".case").live("click", function () {
                $('#Loading').removeClass('hide');
                groupIdsList = [];
                if ($(".case").length == $(".case:checked").length) {
                    $(".SelectAll").attr("checked", "checked");
                    $(".case").removeAttr("checked");
                    $(".case").each(function () {
                        groupIdsList.push(this.value);
                    });
                } else {
                    // $(".case:checked").each(function () {
                    groupIdsList.push(this.value);
                    // });
                }
                var e;
                var noitems;
                isSingleView = false;

                if ($('#btnviewOne').length > 0) {
                    // e = document.getElementById("ddlPageDenomination");
                    noitems = 0;
                }
                else {
                    noitems = 10;
                    isSingleView = true;
                    pageNoForSigleItem = 2;
                    iscasecheck = true;
                }

                var urltogo = "../ItemListView/GetItemListPartial";
                var data1 = { iD: quoteid, type: quoteType, ddlSelectedValue: noitems, pgno: "1", selectionStatus: groupIdsList.toString(), IsSingleView: isSingleView };

                $.ajax({
                    url: urltogo,
                    type: "POST",
                    data: data1,
                    datatype: 'html',
                    success: function (data) {
                        //YesNoMaybeHover();
                        $('#divMain').html('');
                        $('#divMain').html(data);
                        $('#Loading').addClass('hide');
                        for (var i = 0; i < groupIdsList.length; i++) {
                            $(".case").each(function () {
                                if (this.value == groupIdsList.toString().split(',')[i]) {
                                    $(this).attr("checked", "checked");
                                }
                            });
                        }
                        if ($(".case").length == $(".case:checked").length) {
                            $(".SelectAll").attr("checked", "checked");
                            $(".case").removeAttr("checked");
                        }
                        if (Issingleview) {
                            $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');
                        }
                        $('.lableValTD').each(function () {
                            if (this.id == "1") {
                                yesCount = $(this).html().replace('(', '').replace(')', '');
                            }
                            if (this.id == "2") {
                                noCount = $(this).html().replace('(', '').replace(')', '');
                            }
                            if (this.id == "3") {
                                mayBeCount = $(this).html().replace('(', '').replace(')', '');
                            }
                            if (this.id == "5") {
                                newCount = $(this).html().replace('(', '').replace(')', '');
                            }
                        });
                        var allCount = parseInt(yesCount) + parseInt(noCount) + parseInt(mayBeCount) + parseInt(newCount);
                        var calcpercentage = ((parseInt(yesCount)) / (parseInt(allCount))) * 100;
                        var pb1 = $('#pb1').progressbar();
                        pb1.progressbar('value', parseInt(calcpercentage));
                        //// $('.uncheck').addClass('hide');
                        noofpagesOnReturncall = $('#hdnNoofpages').val();
                        $('.popup-with-zoom-anim').magnificPopup({
                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                    }
                });

            });
            //$(".case").live("click", function () {
            //    $('#Loading').removeClass('hide');
            //    groupIdsList = [];
            //    if ($(".case").length == $(".case:checked").length) {
            //        $(".SelectAll").attr("checked", "checked");
            //        $(".case").removeAttr("checked");
            //        $(".case").each(function () {
            //            groupIdsList.push(this.value);
            //        });
            //    } else {
            //        groupIdsList.push(this.value);
            //    }
            //    var e;
            //    var noitems;
            //    var isSingleView = false;

            //    if ($('#btnviewAll').length >0) {
            //       // e = document.getElementById("ddlPageDenomination");
            //        noitems = 0;
            //    }
            //    else {
            //        noitems = 10;
            //        isSingleView = true;
            //        pageNoForSigleItem = 2;
            //        iscasecheck = true;
            //    }
            //    var urltogo = "../ItemListView/GetItemListPartial";
            //    var data1 = { iD: quoteid, type: quoteType, ddlSelectedValue: noitems, pgno: "1", selectionStatus: groupIdsList.toString(), IsSingleView: isSingleView };


            //    $.ajax({
            //        url: urltogo,
            //        type: "POST",
            //        data: data1,
            //        datatype: 'html',
            //        success: function (data) {
            //           // YesNoMaybeHover();
            //            $('#divMain').html('');
            //            $('#divMain').html(data);
            //            for (var i = 0; i < groupIdsList.length; i++) {
            //                $(".case").each(function () {
            //                    if (this.value == groupIdsList.toString().split(',')[i]) {
            //                        $(this).attr("checked", "checked");
            //                    }
            //                });
            //            }
            //            if ($(".case").length == $(".case:checked").length) {
            //                $(".SelectAll").attr("checked", "checked");
            //                $(".case").removeAttr("checked");
            //            }
            //            //$('#gotoPageDiv li a').each(function () {
            //            //    if ((this.title == title) && !((($(this).hasClass('first')) || ($(this).hasClass('last'))))) {
            //            //        $(this).addClass('inactive');
            //            //        $(this).removeAttr("href");
            //            //    }
            //            //    if (gotoPageNo == this.title) {

            //            //        $(this).addClass('inactive');
            //            //        $(this).removeAttr("href");
            //            //    }
            //            //});
            //           // $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');
            //            $('.lableValTD').each(function () {
            //                if (this.id == "1") {
            //                    yesCount = $(this).html().replace('(', '').replace(')', '');
            //                }
            //                if (this.id == "2") {
            //                    noCount = $(this).html().replace('(', '').replace(')', '');
            //                }
            //                if (this.id == "3") {
            //                    mayBeCount = $(this).html().replace('(', '').replace(')', '');
            //                }
            //                if (this.id == "5") {
            //                    newCount = $(this).html().replace('(', '').replace(')', '');
            //                }
            //            });
            //            var allCount = parseInt(yesCount) + parseInt(noCount) + parseInt(mayBeCount) + parseInt(newCount);
            //            var calcpercentage = ((parseInt(yesCount)) / (parseInt(allCount))) * 100;
            //            var pb1 = $('#pb1').progressbar();
            //            pb1.progressbar('value', parseInt(calcpercentage));
            //           // $('.uncheck').addClass('hide');
            //           // noofpagesOnReturncall = $('#hdnNoofpages').val();
            //            $('.popup-with-zoom-anim').magnificPopup({
            //                type: 'inline',
            //                fixedContentPos: false,
            //                fixedBgPos: true,
            //                overflowY: 'auto',
            //                closeBtnInside: true,
            //                preloader: false,
            //                midClick: true,
            //                removalDelay: 300,
            //                mainClass: 'my-mfp-zoom-in',
            //            });
            //        }
            //    });
            //});

            $('#btnviewAll').live("click", function () {
                $('#Loading').removeClass('hide');
                groupIdsList = [];
                Issingleview = false;
                $(".SelectAll").attr("checked", "checked");
                $(".case").removeAttr("checked");
                $(".case").each(function () {
                    groupIdsList.push(this.value);
                });

                if (quoteType == $("#hdnDecisionWizardliText").val() || quoteType == 'DefaultDW' || quoteType == 'LiveCustDW') {
                    id = quoteid;
                    type = quoteType;
                }
                else {
                    id = parseInt(groupId);
                    type = "Group";
                }

                var urltogo = "../ItemListView/GetItemListPartial";
                var data1 = { iD: id, type: type, ddlSelectedValue: "0", pgno: "1", selectionStatus: groupIdsList.toString() };

                currentPageIndex = 1;
                $.ajax({
                    url: urltogo,
                    type: "POST",
                    data: data1,
                    datatype: 'html',
                    success: function (data) {
                        $('#divMain').html('');
                        $('#divMain').html(data);
                        for (var i = 0; i < groupIdsList.length; i++) {
                            $(".case").each(function () {
                                if (this.value == groupIdsList.toString().split(',')[i]) {
                                    $(this).attr("checked", "checked");
                                }
                            });
                        }
                        $('#Loading').addClass('hide');
                        yesNoMaybeStatus();
                        $('.YesButton').die('mouseenter');
                        $('.YesButton').die('mouseleave');

                        $('.MayBeButton').die('mouseenter');
                        $('.MayBeButton').die('mouseleave');

                        $('.NoButton').die('mouseenter');
                        $('.NoButton').die('mouseleave');
                        // RoundYesNoMaybeHover(false);
                        calculateProgressBar();
                        // $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');
                        $('.popup-with-zoom-anim').magnificPopup({
                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                        $('.case').attr('checked', false);
                        $('.SelectAll').attr('checked', true);
                    }
                });
            });

            $('#btnviewOne').live("click", function () {
                $('#Loading').removeClass('hide');
                groupIdsList = [];
                Issingleview = true;
                RoundYesNoMaybeHover(true);
                if ($(".case").length == $(".case:checked").length) {
                    $(".SelectAll").attr("checked", "checked");
                    $(".case").removeAttr("checked");
                    $(".case").each(function () {
                        groupIdsList.push(this.value);
                    });
                } else {
                    $(".case:checked").each(function () {
                        groupIdsList.push(this.value);
                    });
                    if (groupIdsList.length == 0) {
                        groupIdsList.push(5);
                    }
                }

                if (quoteType == $("#hdnDecisionWizardliText").val() || quoteType == 'DefaultDW' || quoteType == 'LiveCustDW') {
                    id = quoteid;
                    type = quoteType;
                }
                else {
                    id = parseInt(groupId);
                    type = "Group";
                }

                var urltogo = "../ItemListView/GetItemListPartial";
                var data1 = { iD: id, type: type, ddlSelectedValue: "10", pgno: "1", selectionStatus: groupIdsList.toString(), IsSingleView: Issingleview };

                $.ajax({
                    url: urltogo,
                    type: "POST",
                    data: data1,
                    datatype: 'html',
                    success: function (data) {
                        $('#divMain').html('');
                        $('#divMain').html(data);
                        if (groupIdsList.length > 1) {
                            $('.SelectAll').attr("checked", "checked");

                        }
                        else {
                            $(".case").each(function () {
                                if (this.value == groupIdsList.toString()) {
                                    $(this).attr("checked", "checked");
                                }
                            });
                        }
                        //yesNoMaybeStatus();
                        calculateProgressBar();
                        $('#Loading').addClass('hide');
                        //  $('.uncheck').addClass('hide');
                        $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');
                        $('.popup-with-zoom-anim').magnificPopup({
                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                    }
                });
            });

            $('#ddlPageDenomination').live('change', function (e) {
                if ($('#ddlPageDenomination').val() != "") {
                    if (parseInt($('#ddlPageDenomination').val()) == 1) {
                        var itemdetails = $("#tblItemDetails tr td").find('table tr td').find('div div a');
                        Issingleview = true;
                        pageNoForSigleItem = 2;
                    }
                    // else {
                    groupIdsList = [];
                    if ($('input.case').is(':checked')) {
                        $(".case:checked").each(function () {
                            groupIdsList.push(this.value);
                            $(this).checked = true;
                        });
                        $(".SelectAll").removeAttr("checked");
                    } else {
                        $(".case").each(function () {
                            groupIdsList.push(this.value);
                        });
                    }

                    var urltogo = '';
                    var data1 = '';

                    var id;
                    var type;

                    id = quoteid;
                    type = quoteType;
                    //if (quoteType == $("#hdnDecisionWizardliText").val() || quoteType == 'DefaultDW') {
                    //    id = quoteid;
                    //    type = quoteType;
                    //}
                    //else {
                    //    id = parseInt(groupId);
                    //    type = "Group";
                    //}

                    urltogo = "../ItemListView/GetItemListPartial";
                    data1 = { iD: id, type: type, ddlSelectedValue: document.getElementById("ddlPageDenomination").value, pgno: "1", selectionStatus: groupIdsList.toString() };

                    currentPageIndex = 1;
                    $.ajax({
                        url: urltogo,
                        type: "POST",
                        data: data1,
                        datatype: 'html',
                        success: function (data) {
                            $('#divMain').html('');
                            $('#divMain').html(data);
                            if (groupIdsList.length > 1) {
                                $('.SelectAll').attr("checked", "checked");
                            }
                            else {
                                $(".case").each(function () {
                                    if (this.value == groupIdsList.toString()) {
                                        $(this).attr("checked", "checked");
                                    }
                                });
                            }
                            //for (var i = 0; i < groupIdsList.length; i++) {
                            //    $(".case").each(function () {
                            //        if (this.value == groupIdsList.toString().split(',')[i]) {
                            //            $(this).attr("checked", "checked");
                            //        }
                            //    });
                            //}
                            //yesNoMaybeStatus();
                            calculateProgressBar();
                            $('.uncheck').addClass('hide');
                            $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');
                            $('.popup-with-zoom-anim').magnificPopup({
                                type: 'inline',
                                fixedContentPos: false,
                                fixedBgPos: true,
                                overflowY: 'auto',
                                closeBtnInside: true,
                                preloader: false,
                                midClick: true,
                                removalDelay: 300,
                                mainClass: 'my-mfp-zoom-in',
                            });
                        }
                    });
                    //}

                }
            });

            $('.gotoPage').live("click", function (e) {
                groupIdsList = [];
                if ($('input.case').is(':checked')) {
                    $(".case:checked").each(function () {
                        groupIdsList.push(this.value);
                        $(this).checked = true;
                        selectionCount = $(this).attr('data-count');
                    });

                    $(".SelectAll").removeAttr("checked");
                } else {
                    $(".case").each(function () {
                        groupIdsList.push(this.value);
                    });

                    selectionCount = $(".SelectAll").attr('data-count');
                }
                var innerClass = $(this).attr('class');


                if (innerClass.indexOf("first") != -1) {

                    if (parseInt(currentPageIndex) == 1) {
                        gotoPageNo = 1;
                    }
                    else {
                        gotoPageNo = parseInt(currentPageIndex) - 1;
                    }
                }

                if (innerClass.indexOf("last") != -1) {
                    if (parseInt(currentPageIndex) <= parseInt(selectionCount) / parseFloat(parseInt(document.getElementById("ddlPageDenomination").value))) {
                        gotoPageNo = parseInt(currentPageIndex) + 1;
                    }
                    else {
                        gotoPageNo = parseInt(currentPageIndex);
                    }
                }
                if (innerClass == "gotoPage") {
                    gotoPageNo = parseInt(this.title);
                }
                $(this).addClass('inactive');
                title = this.title;
                var urltogo = '';
                var data1 = '';

                var id;
                var type;

                //if (quoteType == $("#hdnDecisionWizardliText").val()) {
                //    id = quoteid;
                //    type = quoteType;
                //}
                //else {
                //    id = parseInt(groupId);
                //    type = "Group";
                //}
                id = quoteid;
                type = quoteType;

                urltogo = "../ItemListView/GetItemListPartial";
                data1 = { iD: id, type: type, ddlSelectedValue: document.getElementById("ddlPageDenomination").value, pgno: gotoPageNo, selectionStatus: groupIdsList.toString() }
                $.ajax({
                    url: urltogo,
                    type: "POST",
                    data: data1,
                    datatype: 'html',
                    success: function (data) {
                        $('#divMain').html('');
                        $('#divMain').html(data);
                        $("#hdnCurrentPageIndex").val(gotoPageNo);
                        $('#gotoPageDiv li a').each(function () {
                            if ((this.title == title) && !((($(this).hasClass('first')) || ($(this).hasClass('last'))))) {
                                $(this).addClass('inactive');
                                // $(this).removeAttr("href");
                            }
                            if (gotoPageNo == this.title) {
                                $(this).addClass('inactive');
                                $(this).removeAttr("href");
                            }
                        });

                        if (groupIdsList.length > 1) {
                            $('.SelectAll').attr("checked", "checked");
                        }
                        else {
                            $(".case").each(function () {
                                if (this.value == groupIdsList.toString()) {
                                    $(this).attr("checked", "checked");
                                }
                            });
                        }
                        calculateProgressBar();
                        //yesNoMaybeStatus();
                        currentPageIndex = gotoPageNo;
                        $('.uncheck').addClass('hide');
                        $('.ItemDetailSeries').removeAttr('href').attr('href', '#onScreen-dialog');
                        $('.popup-with-zoom-anim').magnificPopup({
                            type: 'inline',
                            fixedContentPos: false,
                            fixedBgPos: true,
                            overflowY: 'auto',
                            closeBtnInside: true,
                            preloader: false,
                            midClick: true,
                            removalDelay: 300,
                            mainClass: 'my-mfp-zoom-in',
                        });
                    }
                });


            });

            $('.OpenBook').live('click', function () {
                var imgid = $(this).attr('id');
                var imgPath = insideImagePath + imgid + ".jpg";
                $.Dialog({
                    shadow: true,
                    overlay: true,
                    flat: true,
                    icon: '',
                    title: 'Look Inside',
                    padding: 10,
                    content: '<form>' +
                                '<div id="LookInside"  class=""> ' +
                                ' <div class="place-left div-MainHeight20"></div> ' +
                                ' <div class="place-left OpenInsideImage">' +
                                '  <img src="' + imgPath + '" style="height:612px;width:750px;"    />' +
                                '</div>' +
                                ' </div>' +
                                '</form>',
                    onShow: function (_dialog) {
                        $.Dialog.content(content);
                    },

                });
            });


            $('.nav-page').click(function () {
                window.location.reload();
            });


            var ddSelectedValue = $('#ddlPageDenomination').val();
            var noofpagesforsingleitem = $('#hdnNoofpages').val();
            var bookTitle;
            $('.btnSet').live("mousedown", function (e) {
                RoundYesNoMaybeHover(Issingleview);
                if (Issingleview) {
                    $('#Loading').removeClass('hide');
                }
                if (iscasecheck) {
                    noofpagesforsingleitem = noofpagesOnReturncall;
                }
                else {
                    noofpagesforsingleitem = $('#hdnNoofpages').val();
                }

                Issingleview = false;
                if ($('#btnviewAll').length > 0) {
                    Issingleview = true;

                }
                groupIdsList = [];
                if ($('input.case').is(':checked')) {
                    $(".case:checked").each(function () {
                        groupIdsList.push(this.value);
                        $(this).checked = true;
                    });

                    $(".SelectAll").removeAttr("checked");
                } else {
                    $(".case").each(function () {
                        groupIdsList.push(this.value);
                    });
                }
                var dwid = $(this).attr('value');
                var itemid = this.id;
                var itemIdsList = [];
                if (dwid == '6' && $('#dvSet-' + itemid).length) {
                    $('#dvSet-' + itemid).find('.group1').each(function () {
                        if (!$(this).hasClass('div-disable')) {
                            itemIdsList.push($(this).attr('value'));
                        }
                    });
                }
                var currentdwid = $(this).attr('name');
                var AllCount;
                var noCount
                    , yesCount
                    , maybeCount
                    , newCount;

                var qdid = quoteid;
                $.ajax({
                    url: "../ItemListView/UpdateDWSelectionStatus",
                    type: "POST",
                    async: false,
                    data: { DWID: dwid, Qdid: qdid, Itemid: itemid, selectionStatus: groupIdsList.toString(), ddlSelectedValue: "1", pgno: currentPageIndex },
                    datatype: 'html',
                    success: function (data) {
                        var AllCount;
                        var noCount
                            , yesCount
                            , maybeCount
                            , newCount;
                        var model = data;
                        AllCount = model.SelectionCount;
                        noCount = model.noOfNoCount;
                        yesCount = model.noOfYesCount;
                        maybeCount = model.noOfMaybeCount;
                        newCount = model.noOfNewCount;
                        //$('.uncheck').addClass('hide');
                        $('a.' + itemid + '').each(function () {
                            if (($(this).attr('value') == dwid) && this.id == itemid) {
                                if ($(this).attr('value') == 1) {
                                    // $(this).children('img').attr('src', '../Images/YesNoMaybe/Yes.jpg');
                                    $(this).children('img').attr('data-value', 'Yes');
                                    if (Issingleview) {
                                        $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/Yes_Round.jpg').attr('data-value', 'Null');
                                        $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                        $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');
                                    }
                                    else {
                                        if ($(this).hasClass("popup")) {
                                            $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/Yes_Round.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');

                                        } else {
                                            $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/Yes.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew.jpg').attr('data-value', 'Null');
                                        }
                                    }

                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttext-' + itemid + '"]').removeClass('hide').show();
                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttextlink-' + itemid + '"]').hide();
                                }
                                if ($(this).attr('value') == 6) {
                                    for (var i in itemIdsList) {
                                        var itemvalue = itemIdsList[i];

                                        $('.popup[id=' + itemvalue + ']').find('.YesButton').attr('src', '../Images/YesNoMaybe/Yes_Round.jpg').attr('data-value', 'Null');
                                        $('.popup[id=' + itemvalue + ']').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                        $('.popup[id=' + itemvalue + ']').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');

                                        $('a[id=' + itemvalue + ']').find('.YesButton.listview').attr('src', '../Images/YesNoMaybe/Yes.jpg').attr('data-value', 'Null');
                                        $('a[id=' + itemvalue + ']').find('.MayBeButton.listview').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                                        $('a[id=' + itemvalue + ']').find('.NoButton.listview').attr('src', '../Images/YesNoMaybe/NoNew.jpg').attr('data-value', 'Null');

                                        $('a[id=' + itemvalue + '][value=1]').children('img').attr('data-value', 'Yes');
                                        $('div:[id="carttext-' + itemvalue + '"]').removeClass('hide').show();
                                        $('div:[id="carttextlink-' + itemvalue + '"]').addClass('hide').hide();
                                        $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttext-+' + itemvalue + '"]').removeClass('hide').show();
                                        $('div:[id="' + itemvalue + '"]').children('div').children('div').find('div:[id="carttextlink-+' + itemvalue + '"]').addClass('hide').hide();
                                        $('input[type=checkbox][value=' + itemvalue + ']').prop('checked', true);
                                    }
                                }

                                if ($(this).attr('value') == 2) {
                                    //   $(this).children('img').attr('src', '../Images/YesNoMaybe/No.jpg');
                                    $(this).children('img').attr('data-value', 'No');
                                    if (Issingleview) {
                                        $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg').attr('data-value', 'Null');
                                        $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                        $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/No_Round.jpg').attr('data-value', 'Null');
                                    }
                                    else {

                                        if ($(this).hasClass("popup")) {
                                            $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/No_Round.jpg').attr('data-value', 'Null');

                                        } else {
                                            $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/No.jpg').attr('data-value', 'Null');
                                        }
                                    }
                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttext-' + itemid + '"]').hide();
                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttextlink-' + itemid + '"]').removeClass('hide').show();
                                }

                                if ($(this).attr('value') == 3) {
                                    // $(this).children('img').attr('src', '../Images/YesNoMaybe/MayBe.jpg');
                                    $(this).children('img').attr('data-value', 'MayBe');
                                    if (Issingleview) {
                                        $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg').attr('data-value', 'Null');
                                        $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');
                                        $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBe_Round.jpg').attr('data-value', 'Null');
                                    }
                                    else {
                                        if ($(this).hasClass("popup")) {
                                            $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBe_Round.jpg').attr('data-value', 'Null');

                                        } else {
                                            $(this).parent('div').parent('div').find('.YesButton').attr('src', '../Images/YesNoMaybe/YesNew.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.NoButton').attr('src', '../Images/YesNoMaybe/NoNew.jpg').attr('data-value', 'Null');
                                            $(this).parent('div').parent('div').find('.MayBeButton').attr('src', '../Images/YesNoMaybe/MayBe.jpg').attr('data-value', 'Null');
                                        }
                                    }

                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttext-' + itemid + '"]').hide();
                                    $('div:[id="' + itemid + '"]').children('div').children('div').find('div:[id="carttextlink-' + itemid + '"]').removeClass('hide').show();
                                }
                                if (Issingleview == true) {
                                    var nexttableHtml;
                                    $('#pb1').hide();

                                    if (dwid == '6' && $('#dvSet-' + itemid).length) {
                                        for (var i in itemIdsList) {
                                            $('#tblItemDetails .itemDetails').each(function () {
                                                if ($(this).hasClass(itemIdsList[i])) {
                                                    $(this).addClass('setItem');
                                                }
                                            });
                                        }

                                    }

                                    nexttableHtml = $('table.' + itemid + '').next('table').html();
                                    if (nexttableHtml != null) {
                                        $('table.' + itemid + '').addClass('hide');
                                        $('table.' + itemid + '').next('table').removeClass('hide');
                                        bookTitle = $('table.' + itemid + '').next('table').find('.singleItemView').attr('data-title');
                                        $('#ulBreadcrumbCust li:last-child').children('a').html(bookTitle);
                                        if ($('table.' + itemid + '').hasClass('hide')) {
                                            $('#Loading').addClass('hide');
                                        }
                                    }
                                    else if (pageNoForSigleItem <= noofpagesforsingleitem) {
                                        $.ajax({
                                            url: "../ItemListView/GetSingleItemListBySelectionByDW",
                                            type: "POST",
                                            data: { quoteId: quoteid, selectionStatus: groupIdsList.toString(), ddlSelectedValue: ddSelectedValue, pgno: pageNoForSigleItem },
                                            datatype: 'html',
                                            success: function (data) {
                                                if (data != null) {
                                                    $('#ItemListView').html('').html(data);
                                                    $('#gotoPageDiv').hide();
                                                    $('#divBottomList').hide();
                                                    $('#pb1').hide();
                                                    $('.itemDetails').addClass('hide');
                                                    $('.itemDetails:first').removeClass('hide');
                                                    bookTitle = $('.itemDetails:first').find('.singleItemView').attr('data-title');
                                                    $('#ulBreadcrumbCust li:last-child').children('a').html(bookTitle);
                                                    Issingleview = true;
                                                    pageNoForSigleItem++;
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        $('#Loading').addClass('hide');
                                        $('.itemsEnd').removeClass('hide');
                                        $('.itemDetails').addClass('hide');
                                    }
                                }
                            }
                            else {
                                if (Issingleview) {
                                    if ($(this).attr('value') == 1) {
                                        $(this).children('img').attr('src', '../Images/YesNoMaybe/YesNew.jpg');
                                    } else if ($(this).attr('value') == 2) {
                                        $(this).children('img').attr('src', '../Images/YesNoMaybe/NoNew_Round.jpg');
                                    }
                                    else if ($(this).attr('value') == 3) {
                                        $(this).children('img').attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg');
                                    }
                                } else {

                                    if ($(this).attr('value') == 1) {

                                        $('.listview').children('img').attr('src', '../Images/YesNoMaybe/YesNew.jpg');
                                    } else if ($(this).attr('value') == 2) {

                                        $('.listview').children('img').attr('src', '../Images/YesNoMaybe/NoNew.jpg');

                                    }
                                    else if ($(this).attr('value') == 3) {
                                        $('.listview').children('img').attr('src', '../Images/YesNoMaybe/MayBeNew.jpg');
                                    }
                                }
                            }
                        });

                        $('.lableValTD').each(function () {
                            if (this.id == "0") $(this).html('(' + AllCount + ')');
                            if (this.id == "1") $(this).html('(' + yesCount + ')');
                            if (this.id == "2") $(this).html('(' + noCount + ')');
                            if (this.id == "3") $(this).html('(' + maybeCount + ')');
                            if (this.id == "5") $(this).html('(' + newCount + ')');
                        });
                        $("#lblYesPrice").text('$' + model.YesTotalPrice.toFixed(2))
                        $('#SCItemCount sup').html(model.SCItemsCount);
                        $('#SCItemPrice a').html('$' + model.SCItemsPrice.toFixed(2));
                        $('#DWItemCount sup').html(AllCount);
                        // $('.uncheck').addClass('hide');
                        var calcpercentage = ((parseInt(yesCount)) / (parseInt(AllCount))) * 100;
                        var pb1 = $('#pb1').progressbar();
                        pb1.progressbar('value', parseInt(calcpercentage));
                    },
                    error: function (data) { alert(Error); }

                });
            });

            if (quoteType == 'ShoppingCart') {
                $('#pb1').hide();
                $('#tblStatusList tr td').slice(0, 10).hide();
            }

            function calculateProgressBar() {
                var allCount = parseInt(yesCount) + parseInt(noCount) + parseInt(mayBeCount) + parseInt(newCount);
                var calcpercentage = ((parseInt(yesCount)) / (parseInt(allCount))) * 100;
                var pb1 = $('#pb1').progressbar();
                pb1.progressbar('value', parseInt(calcpercentage));
            }
        }

        //Index Page Script-----------------------------------------------------------------------------------------Start
        function indexPageScript() {
            $('.navlihide , .menuHome').hide();
            $('#divMenusCustRole').hide();
            $('.carousel').attr('style', 'height: 279px;');
            $('.roundBtn').on("click", function () {
                var id = this.title;
                $("#slide" + id).attr('style', 'left: 0px;');
            });

            $('.groupName').each(function (i) {
                $(this).addClass('text-center');
            });

            $('.GroupView').each(function () {
                var title = $(this).attr('data-title');
                if (title == 'New This Month') {
                    $(this).parent().html('New This Month').addClass('span3 text-left').attr('style', 'font-size: medium;height:25px');
                }
                if (title == "Top Selling Characters") {
                    $(this).parent().html('Top Selling Characters').addClass('span3 text-left').attr('style', 'font-size: medium;height:25px');
                }
                if (title == "Top Selling Series") {
                    $(this).parent().html('Top Selling Series').addClass('span3 text-left').attr('style', 'font-size: medium;height:25px');
                }
                $(this).addClass('span4 place-right text-left');
            });

            //$('.packagegrp-36').insertBefore('#CategoryText-42');

            //$('.packagegrp-43').insertBefore('.packagegrp-36');
            $('#CategoryText-44').html('Top Selling Packages').attr('style', 'font-size: medium;');;
            $('[class^=divCartStatus]').each(function () {
                $(this).remove();
            });
            $('.seeAllText').each(function () {
                $(this).removeClass('span5').addClass('span7');
            });

            $('.carouselLeft').each(function () {
                $(this).addClass('place-right').parent().attr('style', 'width: 45px;');
            });
            //$('.itemTextWidth').hide();
            //$('[class^=imgTest]').each(function () {
            //    $(this).removeClass('shadow').addClass('titleShadow').attr('style', 'height:135px');
            //});
            //  $('.packageGroup:first').attr('style', 'height:182px');

            $('.singleImgWidth').removeClass('singleImgWidth').addClass('singleHomeImgWidth').removeClass('margin20').addClass('margin5');
            $('.padding40').removeClass('padding40').addClass('padding55');
            $('.slideleftDiv').removeClass('padding10').addClass('padding20');
        }
        //Index Page Script-----------------------------------------------------------------------------------------End

        //Librarian Resources SCript--------------------------------------------------------------------------------Start
        function LibraraianResourcesScript() {
            var scrolled = 0;

            $('.aResCat').click(function () {

                $('.CatItems').removeClass('hide').addClass('hide');
                var d = this.title

                $(document.getElementById(d)).removeClass('hide').addClass('visible shadow');

                setTimeout(function () {
                    $(document.getElementById(d)).removeClass('shadow');
                }, 500);
                scrolled = scrolled + 400;
                $("body").animate({
                    scrollTop: scrolled
                });
                scrolled = 0;
                return false;
            });

            $('.aResCatDetails').click(function () {
                var d = this.title.toLowerCase();
                $.ajax({
                    url: "../LibrarianResources/pdfLoad",
                    type: "POST",
                    data: { path: d },
                    datatype: 'html',
                    success: function (data) {
                        $('#loadingSetView').html('');
                        $('#loadingSetView').html(data);
                    }
                });
            });
        }
        //Librarian Resources Script--------------------------------------------------------------------------------End

        function KPLViewBarcodeScript() {
            $("#Barcode").focus();
            var $trrow = $("#tblKPLItemList tbody tr:first");

            var quoteid = $('#hdnQuoteID').val();
            var deleteQuotePath = $('#hdnDeleteQuotePath').val();
            var addQuotePath = $('#hdnAddQuotePath').val();


            //$("#tblKPLItemList").tablesorter({
            //    widthFixed: true,
            //    widgets: ['stickyHeaders']
            //});

            var tbl = jQuery('#tblKPLItemList');
            var tbl2 = jQuery('#tblKPLItemList-sticky');

            for (var i = 0; i <= tbl.find('th').length; i++) {

                var rowIndex = tbl.find('th.hide[data-column=' + [i] + ']').index();
                if (rowIndex != -1) {
                    tbl.find('input.tablesorter-filter[data-column=' + [i] + ']').parent().addClass('hide');
                    tbl2.find('input.tablesorter-filter[data-column=' + [i] + ']').parent().addClass('hide');
                }
            }

        }
        function SubmitQuoteFunctionality() {
            var submitQuotePath = $('#hdnSubmiQuotePath').val();
            var selectedQuoteID;
            var quoteDetails = null;
            if (pageViewTitle == "QuoteView") {
                selectedQuoteID = $("#hdnQuoteID").val();
                var gridData = $('.gridQuotescrollContent').children().html();
                if (gridData == null) {
                    quoteDetails = 0;
                }
            }
            else {
                $('.Quote').each(function () {
                    if ($(this).is(':checked')) {
                        selectedQuoteID = $(this).attr('data-quoteID');
                        quoteDetails = $('#tblQuote').find('tbody').find('#Quote-' + selectedQuoteID).find('td[id=quoteItems]').text();
                        // quoteDetails = $(this).attr('value').split('%')[2];
                    }
                });
            }
            if (quoteDetails == 0) {
                $('#SubmitQuote-dialog').css("height", "75px").css("width", "335px");
                $('#loadingSubmitQuoteView').html('');
                $(".submitClose").removeClass("hide");
                $('#loadingSubmitQuoteView').html('<div class="margin10 nrm">A Quote having 0 items cannot be submitted.</div>');
            }
            else {
                $(".submitClose").addClass("hide");
                $('#loadingSubmitQuoteView').html('<div class="margin10">Loading...</div>');
                if ($.browser.mozilla) {
                    $('#SubmitQuote-dialog').css("height", "530px").css("width", "815px");
                    // $('#Catalog').css("margin-top","-20px");
                }
                else { $('#SubmitQuote-dialog').css("height", "500px").css("width", "815px"); }

                $.ajax({
                    url: submitQuotePath,
                    type: "POST",
                    datatype: 'html',
                    data: { quoteID: selectedQuoteID },
                    success: function (data) {
                        submitQuoteJsonModel = data;
                        $('#loadingSubmitQuoteView').html(data);
                    }
                });
            }
            $.magnificPopup.close();
        }

        function RapidEntryFunctionality(control) {
            var quoteid;
            var RapidEntryPath = $('#hdnRapidEntryPath').val();
            var type = control.attr('data-type');
            if (type == "DW") {
                if ($('#tblDW-0').find('tbody').find('tr').find('td').find('#assignDWChkBx:checked').length == 1) {
                    quoteid = $('#tblDW-0').find('tbody').find('tr').find('td').find('#assignDWChkBx:checked').attr('data-quoteid');
                }
            }
            else {
                if ($('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx:checked').length == 1) {
                    quoteid = $('#tblQuote').find('tbody').find('tr').find('td').find('#assignChkBx:checked').attr('data-quoteid');
                }
            }
            $.ajax({
                url: RapidEntryPath,
                type: "POST",
                datatype: 'html',
                data: { quoteID: quoteid },
                success: function (data) {
                    $('#loadingSubmitQuoteView').html(data);
                    $('#Barcode').focus();
                    if ($('#NoOfTitles').val() == '0' || $('#NoOfBooks').val() == '0' || $('#QuoteTotalBefortaxandCatlog').val() == '0') {
                        $('#RapidEntrySubmitOrder').attr("disabled", 'disabled');
                    }
                    else {
                        $('#RapidEntrySubmitOrder').removeAttr("disabled", 'disabled');
                    }
                }
            });
        }

        function YesNoMaybeHover() {
            $('.YesButton').live('mouseenter', function () {
                $(this).attr('src', '../Images/YesNoMaybe/YesHover.jpg');

            }).live('mouseleave', function () {
                if ($(this).attr('data-value') == 'Yes') {
                    $(this).attr('src', '../Images/YesNoMaybe/Yes.jpg');
                }
                else {
                    $(this).attr('src', '../Images/YesNoMaybe/YesNew.jpg');
                }
            });
            $('.MayBeButton').live('mouseenter', function () {
                $(this).attr('src', '../Images/YesNoMaybe/MayBeHover.jpg');
            }).live('mouseleave', function () {
                if ($(this).attr('data-value') == 'MayBe') {
                    $(this).attr('src', '../Images/YesNoMaybe/MayBe.jpg');
                }
                else {
                    $(this).attr('src', '../Images/YesNoMaybe/MayBeNew.jpg');
                }
            });
            $('.NoButton').live('mouseenter', function () {
                $(this).attr('src', '../Images/YesNoMaybe/NoHover.jpg');
            }).live('mouseleave', function () {
                if ($(this).attr('data-value') == 'No') {
                    $(this).attr('src', '../Images/YesNoMaybe/No.jpg');
                }
                else {
                    $(this).attr('src', '../Images/YesNoMaybe/NoNew.jpg');
                }
            });
            $('.YesToAllButton').live('mouseenter', function () {
                $(this).attr('src', '../Images/YesNoMaybe/YesToAllHover.jpg');
            }).live('mouseleave', function () {
                $(this).attr('src', '../Images/YesNoMaybe/YesToAllNew.jpg');
            });
        }



        function RoundYesNoMaybeHover(Issingle) {
            if (Issingle) {
                $('.YesButton').live('mouseenter', function () {
                    $(this).attr('src', '../Images/YesNoMaybe/YesHover.jpg');
                }).live('mouseleave', function () {
                    if ($(this).attr('data-value') == 'Yes') {
                        $(this).attr('src', '../Images/YesNoMaybe/Yes_Round.jpg');
                    }
                    else {
                        $(this).attr('src', '../Images/YesNoMaybe/YesNew_Round.jpg');
                    }
                });
                $('.MayBeButton').live('mouseenter', function () {
                    $(this).attr('src', '../Images/YesNoMaybe/MayBeHover.jpg');
                }).live('mouseleave', function () {
                    if ($(this).attr('data-value') == 'MayBe') {
                        $(this).attr('src', '../Images/YesNoMaybe/MayBe_Round.jpg');
                    }
                    else {
                        $(this).attr('src', '../Images/YesNoMaybe/MayBeNew_Round.jpg');
                    }
                });
                $('.NoButton').live('mouseenter', function () {
                    $(this).attr('src', '../Images/YesNoMaybe/NoHover.jpg');
                }).live('mouseleave', function () {
                    if ($(this).attr('data-value') == 'No') {
                        $(this).attr('src', '../Images/YesNoMaybe/NO_round.jpg');
                    }
                    else {
                        $(this).attr('src', '../Images/YesNoMaybe/NONew_Round.jpg');
                    }
                });
                $('.YesToAllButton').live('mouseenter', function () {
                    $(this).attr('src', '../Images/YesNoMaybe/YesToAllHover.jpg');
                }).live('mouseleave', function () {
                    $(this).attr('src', '../Images/YesNoMaybe/YesToAllNew.jpg');
                });
            }

        }

    });
})(jQuery);