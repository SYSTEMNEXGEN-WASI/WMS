$(document).ready(function (e) {
    $('input[id$="txtDiscountParts"]').keyup(function (e) {
        try {
            var grossAmt = $('input[id$="txttotPartsLub"]').val();
            var PAmt = $('input[id$="txtDiscountParts"]').val();
            var LAmt = $('input[id$="txtDiscLabor"]').val();
            var totAmtCustomer = $('input[id$="txtTotalAmtCustomer"]').val();
            var subtot = $('input[id$="txtSubTotal"]').val();

            if (grossAmt == "") grossAmt = 0; if (PAmt == "") PAmt = 0; if (LAmt == "") LAmt = 0;

            if (parseFloat(PAmt) == 0 | parseFloat(grossAmt) < parseFloat(PAmt)) {
                $('input[id$="txtDiscPercentPart"]').val('');
            }
            else if (parseFloat(grossAmt) > 0 & parseFloat(PAmt) > 0) {
                var roundVal = (parseFloat(PAmt) / parseFloat(grossAmt)) * 100;
                $('input[id$="txtDiscPercentPart"]').val(roundVal.toFixed(2));
                //$('input[id$="txtDiscPercentPart"]').val(roundVal.toFixed(0));
            }
            $('input[id$="txtTotalAmtCustomer"]').val(parseFloat(subtot) - (parseFloat(PAmt) + parseFloat(LAmt)))
        }
        catch (e) { $('input[id$="txtDiscountParts"]').val(0); $('input[id$="txtDiscPercentPart"]').val(0); }
    });

    $('input[id$="txtDiscPercentPart"]').keyup(function (e) {
        try {
            var grossAmt = $('input[id$="txttotPartsLub"]').val();
            var PPer = $('input[id$="txtDiscPercentPart"]').val();
            var LAmt = $('input[id$="txtDiscLabor"]').val();
            var totAmtCustomer = $('input[id$="txtTotalAmtCustomer"]').val();
            var subtot = $('input[id$="txtSubTotal"]').val();

            if (grossAmt == "") grossAmt = 0; if (PPer == "") PPer = 0; if (LAmt == "") LAmt = 0;

            if (parseFloat(PPer) == 0 | parseFloat(PPer) > 100) {
                $('input[id$="txtDiscountParts"]').val('');
            }
            else if (parseFloat(grossAmt) > 0 & parseFloat(PPer) > 0) {
                var roundVal = (parseFloat(grossAmt) * (parseFloat(PPer) / 100));
                $('input[id$="txtDiscountParts"]').val(roundVal.toFixed(2));
               // $('input[id$="txtDiscountParts"]').val(roundVal.toFixed(0));
            }

            if ($('input[id$="txtDiscountParts"]').val() == "") $('input[id$="txtTotalAmtCustomer"]').val(parseFloat(subtot) - parseFloat(LAmt));
            else $('input[id$="txtTotalAmtCustomer"]').val(parseFloat(subtot) - (parseFloat($('input[id$="txtDiscountParts"]').val()) + parseFloat(LAmt)));
        }
        catch (e) { $('input[id$="txtDiscountParts"]').val(0); $('input[id$="txtDiscPercentPart"]').val(0); }
    });

    //////Labour
    $('input[id$="txtDiscLabor"]').keyup(function (e) {
        try {
            var grossAmt = $('input[id$="txttotJobSublet"]').val();
            var LAmt = $('input[id$="txtDiscLabor"]').val();
            var PAmt = $('input[id$="txtDiscountParts"]').val();
            var totAmtCustomer = $('input[id$="txtTotalAmtCustomer"]').val();
            var subtot = $('input[id$="txtSubTotal"]').val();

            if (grossAmt == "") grossAmt = 0; if (LAmt == "") LAmt = 0; if (PAmt == "") PAmt = 0

            if (parseFloat(grossAmt) < parseFloat(LAmt) | parseFloat(LAmt) == 0) {
                $('input[id$="txtDiscLaborPercent"]').val('');
            }
            else if (parseFloat(grossAmt) > 0 & parseFloat(LAmt) > 0) {
                var roundVal = (parseFloat(LAmt) / parseFloat(grossAmt)) * 100;
                $('input[id$="txtDiscLaborPercent"]').val(roundVal.toFixed(2));
                //$('input[id$="txtDiscLaborPercent"]').val(roundVal.toFixed(0));
            }

            $('input[id$="txtTotalAmtCustomer"]').val(parseFloat(subtot) - (parseFloat(LAmt) + parseFloat(PAmt)))
        }
        catch (e) { $('input[id$="txtDiscLabor"]').val(0); $('input[id$="txtDiscLaborPercent"]').val(0); }
    });

    $('input[id$="txtDiscLaborPercent"]').keyup(function (e) {
        try {

            var grossAmt = $('input[id$="txttotJobSublet"]').val();
            var LPer = $('input[id$="txtDiscLaborPercent"]').val();
            var PAmt = $('input[id$="txtDiscountParts"]').val();
            var totAmtCustomer = $('input[id$="txtTotalAmtCustomer"]').val();
            var subtot = $('input[id$="txtSubTotal"]').val();

            if (grossAmt == "") grossAmt = 0; if (LPer == "") LPer = 0; if (PAmt == "") PAmt = 0

            if (parseFloat(LPer) > 100 | parseFloat(LPer) == 0) {
                $('input[id$="txtDiscLabor"]').val('');
            }
            else if (parseFloat(grossAmt) > 0 & parseFloat(LPer) > 0) {
                var roundVal = (parseFloat(grossAmt) * (parseFloat(LPer) / 100));
                $('input[id$="txtDiscLabor"]').val(roundVal.toFixed(2));
                //$('input[id$="txtDiscLabor"]').val(roundVal.toFixed(0));
            }

            if ($('input[id$="txtDiscLabor"]').val() == "") $('input[id$="txtTotalAmtCustomer"]').val(parseFloat(subtot) - parseFloat(PAmt));
            else $('input[id$="txtTotalAmtCustomer"]').val(parseFloat(subtot) - (parseFloat($('input[id$="txtDiscLabor"]').val()) + parseFloat(PAmt)));
        }
        catch (e) { $('input[id$="txtDiscLabor"]').val(0); $('input[id$="txtDiscLaborPercent"]').val(0); }
    });
});