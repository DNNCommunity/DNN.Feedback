<%@ Control language="vb" CodeBehind="Settings.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Feedback.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="dnnForm FeedbackSettings">
    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="dnnFormMessage dnnFormValidationSummary"
        EnableClientScript="true" DisplayMode="List" />
    <h2 class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("scnEmail")%></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:label id="plSendTo" runat="server" controlname="txtSendTo" suffix=":"></dnn:label>
            <asp:textbox id="txtSendTo" runat="server" width="300px" cssclass="NormalTextBox"></asp:textbox>
			<asp:regularexpressionvalidator id="valSendTo" resourcekey="valSendTo.ErrorMessage" runat="server" cssclass="dnnFormMessage dnnFormError" controltovalidate="txtSendTo"
				errormessage="<br/>Email Must be Valid" validationexpression="(([\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+);?\s?)+" display="Dynamic"></asp:regularexpressionvalidator>
        </div>
        <div class="dnnFormItem">
		    <dnn:label id="plSendToRoles" runat="server" controlname="dgSelectedRoles" suffix=":"></dnn:label>
		    <dnn:RolesSelectionGrid runat="server" ID="dgSelectedRoles" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plSendFrom" runat="server" controlname="txtSendFrom" suffix=":"></dnn:label>
		    <asp:textbox id="txtSendFrom" runat="server" width="300px" cssclass="NormalTextBox" columns="35" maxlength="100"></asp:textbox>
			<asp:regularexpressionvalidator id="valSendFrom" resourcekey="valSendFrom.ErrorMessage" runat="server" cssclass="dnnFormMessage dnnFormError" controltovalidate="txtSendFrom"
				errormessage="<br/>Email Must be Valid" validationexpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" display="Dynamic"></asp:regularexpressionvalidator>
        </div>
        <div class="dnnFormItem">
		    <dnn:label id="plSendCopy" runat="server" controlname="chkSendCopy" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkSendCopy" runat="server" cssclass="normal" AutoPostBack="true"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
		    <dnn:label id="plOptout" runat="server" controlname="chkOptout" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkOptout" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
		    <dnn:label id="plAsync" runat="server" controlname="chkAsync" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkAsync" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
    </fieldset>
    <h2 class="dnnFormSectionHead"><a href="" ><%=LocalizeString("scnCategories")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
		    <dnn:label id="plCategory" runat="server" controlname="cboCategory" suffix=":"></dnn:label>
	        <asp:DropDownList id="cboCategory" runat="server" cssclass="normal"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCategorySelectable" runat="server" controlname="chkCategory" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCategory" runat="server" cssclass="normal"></asp:CheckBox>
            <asp:CustomValidator id="valCategory" runat="server" Display="Dynamic" cssclass="dnnFormMessage dnnFormError"
                resourcekey="valEmptyCategoryListMsg.ErrorMessage" EnableViewState="false"></asp:CustomValidator>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCategoryReq" runat="server" controlname="chkCategoryReq" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCategoryReq" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plUseCategoryAsEmail" runat="server" controlname="chkCategoryMailto" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCategoryMailto" runat="server" cssclass="normal"></asp:CheckBox>
            <asp:CustomValidator id="valCategoryMailto" runat="server" Display="Dynamic" cssclass="dnnFormMessage dnnFormError"
                resourcekey="valEmptyCategoryMsg.ErrorMessage" EnableViewState="false"></asp:CustomValidator>
        </div>
    </fieldset>
    <h2 class="dnnFormSectionHead"><a href="" ><%=LocalizeString("scnFields")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
	        <dnn:label id="plLabelDisplay" runat="server" controlname="rblLabelDisplay" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblLabelDisplay" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal">
	            <asp:ListItem Text="Same Line As Field" Value="1" ResourceKey="liLabelSameLine"></asp:ListItem>
	            <asp:ListItem Text="Above Field" Value="2" ResourceKey="liLabelAboveField"></asp:ListItem>
	        </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plWidth" runat="server" controlname="txtWidth" suffix=":"></dnn:label>
            <asp:textbox id="txtWidth" runat="server" width="100px" cssclass="NormalTextBox NumberBox" columns="35" maxlength="100"></asp:textbox>
       		<asp:regularexpressionvalidator id="valWidth" resourcekey="valWidth.ErrorMessage" runat="server" cssclass="dnnFormMessage dnnFormError" controltovalidate="txtWidth"
			     errormessage="" validationexpression="^\d{1,}$|(^(100|\d{1,2}((\.\d{1,2})?)?)%$)" display="Dynamic"></asp:regularexpressionvalidator>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plSubject" runat="server" controlname="txtSubject" suffix=":"></dnn:label>
	        <asp:DropDownList id="cboSubject" runat="server" cssclass="normal"/>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plSubjectEditField" runat="server" controlname="rblSubjectEdit" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblSubjectEditField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal">
	            <asp:ListItem Text="List" Value="1" ResourceKey="liSubjectList"></asp:ListItem>
	            <asp:ListItem Text="Text Box" Value="2" ResourceKey="liSubjectTextBox"></asp:ListItem>
	            <asp:ListItem Text="Hidden" Value="3" ResourceKey="liSubjectHidden"></asp:ListItem>
	        </asp:RadioButtonList>
        </div>
         <div class="dnnFormItem">
	        <dnn:label id="plSubjectEditFieldPlaceholder" runat="server" controlname="rblSubjectEditPlacehoholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtSubjectEditFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plEmailField" runat="server" controlname="rblEmailField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblEmailField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plEmailFieldPlaceholder" runat="server" controlname="rblEmailFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtEmailFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plEmailConfirmField" runat="server" controlname="rblEmailConfirmField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblEmailConfirmField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
         <div class="dnnFormItem">
	        <dnn:label id="plEmailConfirmFieldPlaceholder" runat="server" controlname="rblEmailConfirmFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtEmailConfirmFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plNameField" runat="server" controlname="rblNameField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblNameField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
         <div class="dnnFormItem">
	        <dnn:label id="plNameFieldPlaceholder" runat="server" controlname="rblNameFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtNameFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plStreetField" runat="server" controlname="rblStreetField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblStreetField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
         <div class="dnnFormItem">
	        <dnn:label id="plStreetFieldPlaceholder" runat="server" controlname="rblStreetFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtStreetFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCityField" runat="server" controlname="rblCityField" suffix=":"></dnn:label>
            <asp:RadioButtonList id="rblCityField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
         <div class="dnnFormItem">
	        <dnn:label id="plCityFieldPlaceholder" runat="server" controlname="rblCityFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtCityFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plRegionField" runat="server" controlname="rblRegionField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblRegionField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plRegionFieldPlaceholder" runat="server" controlname="rblRegionFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtRegionFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCountryField" runat="server" controlname="rblCountryField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblCountryField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
         <div class="dnnFormItem">
	        <dnn:label id="plCountryFieldPlaceholder" runat="server" controlname="rblCountryFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtCountryFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plPostalCodeField" runat="server" controlname="rblPostalCodeField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblPostalCodeField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plPostalCodeFieldPlaceholder" runat="server" controlname="rblPostalCodeFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtPostalCodeFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plTelephoneField" runat="server" controlname="rblTelephoneField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblTelephoneField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plTelephoneFieldPlaceholder" runat="server" controlname="rblTelephoneFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtTelephoneFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plMessageField" runat="server" controlname="rblMessageField" suffix=":"></dnn:label>
	        <asp:RadioButtonList id="rblMessageField" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plMessageFieldPlaceholder" runat="server" controlname="rblMessageFieldPlaceholder" suffix=":"></dnn:label>
	        <asp:textbox id="txtMessageFieldPlaceholder" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="1000"></asp:textbox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plEmailRegex" runat="server" controlname="txtEmailRegex" suffix=":"></dnn:label>
            <asp:textbox id="txtEmailRegex" runat="server" width="400px" cssclass="dnnFormRequired NormalTextBox" columns="200" maxlength="200"></asp:textbox><br />
            <asp:Button ID="btnResetEmailRegex" runat="server" CssClass="CommandButton" CausesValidation="False" Text="Reset Default" ResourceKey="btnResetDefault" />
            <asp:requiredfieldvalidator id="valEmailRegex" runat="server" cssclass="dnnFormMessage dnnFormError" Display="Dynamic" errormessage="<br />Email Regex is required."
		            controltovalidate="txtEmailRegex" resourcekey="valEmailRegex.Error"></asp:requiredfieldvalidator>		           
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plPostalCodeRegex" runat="server" controlname="txtPostalCodeRegex" suffix=":"></dnn:label>
            <asp:textbox id="txtPostalCodeRegex" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="200"></asp:textbox><br />
            <asp:Button ID="btnResetPostalCodeRegex" runat="server" CssClass="CommandButton" CausesValidation="False" Text="Reset Default" ResourceKey="btnResetDefault" />
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plTelephoneRegex" runat="server" controlname="txtTelephoneRegex" suffix=":"></dnn:label>
            <asp:textbox id="txtTelephoneRegex" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="200"></asp:textbox><br />
            <asp:Button ID="btnResetTelephoneRegex" runat="server" CssClass="CommandButton" CausesValidation="False" Text="Reset Default" ResourceKey="btnResetDefault" />
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plRows" runat="server" controlname="txtrows" suffix=":"></dnn:label>
		    <asp:textbox id="txtrows" runat="server" width="100px" cssclass="NormalTextBox NumberBox" columns="35" maxlength="100"></asp:textbox>
	        <asp:regularexpressionvalidator id="valRows" resourcekey="valRows.ErrorMessage" controltovalidate="txtrows" validationexpression="^[1-9]+[0-9]*$"
		        display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br/>Rows Must Be A Valid Integer" runat="server" />
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plMaxMessage" runat="server" controlname="txtMaxMessage" suffix=":"></dnn:label>
		    <asp:textbox id="txtMaxMessage" runat="server" width="100px" cssclass="NormalTextBox NumberBox" columns="35" maxlength="100"></asp:textbox>
	        <asp:regularexpressionvalidator id="valMaxMessage" resourcekey="valMaxMessage.ErrorMessage" controltovalidate="txtMaxMessage" validationexpression="^[1-9]+[0-9]*$"
		        display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br/>Max Message Length Must Be A Valid Integer" runat="server" />
        </div>
    </fieldset>
    <h2 class="dnnFormSectionHead"><a href="" ><%=LocalizeString("scnSubmission")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="plCaptchaVisibility" runat="server" controlname="rblCaptchaVisibility" suffix=":"></dnn:label>
            <asp:RadioButtonList ID="rblCaptchaVisibility" runat="server" CssClass="dnnFormRadioButtons" RepeatDirection="Horizontal">
                <asp:ListItem Text="All Users" resourcekey="AllUsers" Value="1"></asp:ListItem>
                <asp:ListItem Text="Anonymous Users Only" resourcekey="AnonymousUsers" Value="2"></asp:ListItem>
                <asp:ListItem Text="Disabled" resourcekey="Disabled" Value="3"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plNoCaptcha" runat="server" controlName="chkNoCaptcha" suffix=":"></dnn:label>
            <asp:CheckBox ID="chkNoCaptcha" runat="server" CssClass="normal" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkNoCaptcha_CheckedChanged" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plNoCaptchaSiteKey" runat="server" controlName="txtNoCaptchaSiteKey" suffix=":"></dnn:label>
            <asp:TextBox ID="txtNoCaptchaSiteKey" runat="server" CssClass="normal" ></asp:TextBox>
            <asp:RequiredFieldValidator id="valNoCaptchaSiteKey" resourcekey="valNoCaptchaSiteKey.ErrorMessage" ControlToValidate="txtNoCaptchaSiteKey" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ErrorMessage="<br />If you enable google NoCaptcha, you need to provide the Site Key" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plNoCaptchaSecretKey" runat="server" controlName="txtNoCaptchaSecretKey" suffix=":"></dnn:label>
            <asp:TextBox ID="txtNoCaptchaSecretKey" runat="server" CssClass="normal"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valNoCaptchaSecretKey" resoucekey="valNoCaptchaSecretKey.ErrorMessage" ControlToValidate="txtNoCaptchaSecretKey" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ErrorMessage="<br />If you enable google NoCaptcha, you need to provide the Secret Key" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plRepeatSubmissionFilter" runat="server" controlname="rblRepeatSubmissionFilter" suffix=":"></dnn:label>
            <asp:RadioButtonList ID="rblRepeatSubmissionFilter" runat="server" CssClass="dnnFormRadioButtons" RepeatDirection="Horizontal" RepeatColumns="2">
                <asp:ListItem Text="No Filtering" resourcekey="NoFiltering" Value="1"></asp:ListItem>
                <asp:ListItem Text="DotNetNuke UserID" resourcekey="UserIDFilter" Value="2"></asp:ListItem>
                <asp:ListItem Text="User IP Address" resourcekey="UserIPFilter" Value="3"></asp:ListItem>
                <asp:ListItem Text="Email Address" resourcekey="UserEmailFilter" Value="4"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plMinSubmissionInteval" runat="server" controlname="txtMinSubmissionInteval" suffix=":"></dnn:label>
	        <asp:textbox id="txtMinSubmissionInteval" runat="server" width="100px" cssclass="NormalTextBox NumberBox" columns="6" maxlength="6"></asp:textbox>
            <asp:regularexpressionvalidator id="valMinSubmissionInteval" resourcekey="valMinSubmissionInteval.ErrorMessage" controltovalidate="txtMinSubmissionInteval" validationexpression="^\d{1,5}$"
	            display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br/>Minimum Submission Inteval Must Be Integer 0-99999" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plDuplicateSubmission" runat="server" controlname="chkDuplicateSubmission" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkDuplicateSubmission" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="plRedirectTabOnSubmission" runat="server" ControlName="ddlRedirectTabOnSubmission" Suffix=":" />
            <asp:DropDownList ID="ddlRedirectTabOnSubmission" runat="server"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plAkismetEnable" runat="server" controlname="chkAkismetEnable" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkAkismetEnable" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plAkismetKey" runat="server" controlname="txtAkismetKey" suffix=":"></dnn:label>
            <asp:textbox id="txtAkismetKey" runat="server" width="400px" cssclass="NormalTextBox" columns="200" maxlength="200"></asp:textbox><br />
            <asp:CustomValidator id="valAkismetKey" runat="server" Display="Dynamic" cssclass="dnnFormMessage dnnFormError"
                resourcekey="valInvalidAkismetKey.ErrorMessage" EnableViewState="false"></asp:CustomValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="plAkismetSendModerator" runat="server" controlname="chkAkismetSendModerator" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkAkismetSendModerator" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
    </fieldset>
    <h2 class="dnnFormSectionHead"><a href="" ><%=LocalizeString("scnModeration")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
    	    <dnn:label id="plScope" runat="server" controlname="rblScope" suffix=":"></dnn:label>
		    <asp:RadioButtonList ID="rblScope" runat="server" CssClass="dnnFormRadioButtons" RepeatDirection="Vertical">
    		    <asp:ListItem Text="Instance (for this Feedback module only)" resourcekey="Instance" Value="1"></asp:ListItem>
			        <asp:ListItem Text="Portal (across all Feedback module of portal)" resourcekey="Portal" Value="2"></asp:ListItem>
			    </asp:RadioButtonList>
        </div>
        <div id="divOrphanedData" runat="server" class="dnnFormItem">
            <dnn:label id="plOrphanedData" runat="server" controlname="dgOrphanedData" suffix=":"></dnn:label>
            <asp:DataGrid ID="dgOrphanedData" runat="server" AutoGenerateColumns="false" GridLines="None" CellPadding="2">
                <HeaderStyle CssClass="DataGrid_Header" HorizontalAlign="Center" />
                <ItemStyle CssClass="DataGrid_Item" HorizontalAlign="Center" />
                <Columns>
                    <asp:BoundColumn DataField="Key" HeaderText="ModuleID"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Value" HeaderText="Items"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid><asp:Button ID="cmdDeleteOrphanedData" ResourceKey="cmdDeleteOrphanedData" CausesValidation="false" runat="server" />
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plModerated" runat="server" controlname="chkModerated" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkModerated" runat="server" cssclass="normal" AutoPostBack="true"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plModerationAdminEmails" runat="server" controlname="chkModerationAdminEmails" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkModerationAdminEmails" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div id="divEmailOnly" runat="server" class="dnnFormItem">
	        <dnn:label id="plEmailOnly" runat="server" controlname="chkEmailOnly" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkEmailOnly" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div id="divSendWhenPublished" runat="server" class="dnnFormItem">
	        <dnn:label id="plSendWhenPublished" runat="server" controlname="chkSendWhenPublished" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkSendWhenPublished" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div id="divModerationCategories" runat="server" class="dnnFormItem">
            <dnn:label id="plModerationCategory" runat="server" controlname="cboModerationCategory" suffix=":"></dnn:label>
	        <asp:CheckBoxList id="cblModerationCategories" runat="server" cssclass="dnnFormRadioButtons" RepeatColumns="2" RepeatDirection="Horizontal"></asp:CheckBoxList>
        </div>
        <div id="divUnmoderatedStatus" runat="server" class="dnnFormItem">
            <dnn:Label ID="plUnmoderatedStatus" runat="server" ControlName="ddlUnmoderatedStatus" Suffix=":" />
            <asp:DropDownList ID="ddlUnmoderatedStatus" runat="server" CssClass="Normal"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="plPrintTemplate" runat="server" ControlName="txtPrintTemplate" Suffix=":"></dnn:Label>
            <asp:TextBox ID="txtPrintTemplate" CssClass="NormalTextBox" Width="425" TextMode="MultiLine" Rows="10" MaxLength="2000" runat="server" /><br />
            <asp:Button ID="btnLoadDefaultPrintTemplate" runat="server" Text="Reset Default" CausesValidation="False" CssClass="CommandButton" resourcekey="btnResetDefault"></asp:Button>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plPrintAction" runat="server" controlname="rblPrintAction" suffix=":"></dnn:label>
	        <asp:RadioButtonList ID="rblPrintAction" runat="server" CssClass="dnnFormRadioButtons" RepeatDirection="Horizontal">
	            <asp:ListItem Text="In-Line (Same Page)" resourcekey="InLine" Value="1"></asp:ListItem>
	            <asp:ListItem Text="Popup (New Page)" resourcekey="Popup" Value="2"></asp:ListItem>
	        </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="plModerationPageSize" runat="server" controlname="ddlModerationPageSize" suffix=":"/>
            <asp:DropDownList ID="ddlModerationPageSize" CssClass="NormalTextBox" Width="200px" runat="server">
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="20" Value="20" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />
            </asp:DropDownList>
        </div>

    </fieldset>
    <h2 class="dnnFormSectionHead"><a href="" ><%=LocalizeString("scnCleanup")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupPending" runat="server" controlname="chkCleanupPending" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCleanupPending" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupPrivate" runat="server" controlname="chkCleanupPrivate" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCleanupPrivate" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupPublished" runat="server" controlname="chkCleanupPublished" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCleanupPublished" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupArchived" runat="server" controlname="CleanupArchived" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCleanupArchived" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupSpam" runat="server" controlname="CleanupSpam" suffix=":"></dnn:label>
	        <asp:CheckBox id="chkCleanupSpam" runat="server" cssclass="normal"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupDaysBefore" runat="server" controlname="txtCleanupDaysBefore" suffix=":"></dnn:label>
		    <asp:textbox id="txtCleanupDaysBefore" runat="server" width="100px" cssclass="NormalTextBox NumberBox" columns="35" maxlength="100"></asp:textbox>
	        <asp:regularexpressionvalidator id="Regularexpressionvalidator1" resourcekey="valCleanupDaysBefore.ErrorMessage" controltovalidate="txtCleanupDaysBefore" validationexpression="^[1-9]+[0-9]*$"
		        display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br/>Days Before Must Be A Valid Integer" runat="server" />
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="plCleanupMaxEntries" runat="server" controlname="chkCleanupMaxEntries" suffix=":"></dnn:label>
		    <asp:textbox id="txtCleanupMaxEntries" runat="server" width="100px" cssclass="NormalTextBox NumberBox" columns="35" maxlength="100"></asp:textbox>
	        <asp:regularexpressionvalidator id="Regularexpressionvalidator2" resourcekey="valCleanupMaxEntries.ErrorMessage" controltovalidate="txtCleanupMaxEntries" validationexpression="^[1-9]+[0-9]*$"
		        display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br/>Max Entries Must Be A Valid Integer" runat="server" />
        </div>
    </fieldset>
</div>