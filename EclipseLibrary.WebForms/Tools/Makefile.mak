﻿# This make file is executed as part of the prebuild step. It concatenates all javascript files into a single file

# Versions of JQuery being used. Used to generate script file names e.g. jquery-1.3.2.min.js
JQUERYVER=1.8.2
JQUERYUIVER=1.10.0

SCRIPTFILES = $(ProjectDir)JQuery\Scripts\json2.js \
	$(ProjectDir)JQuery\Scripts\CommonScripts.js \
	$(ProjectDir)JQuery\Dialog\AjaxDialog.js \
	$(ProjectDir)JQuery\GridViewEx\GridViewEx.js \
	$(ProjectDir)JQuery\Input\Cascade.js \
	$(ProjectDir)JQuery\Input\TextBoxEx.js \
	$(ProjectDir)JQuery\Input\RadioButtonListEx.js \
	$(ProjectDir)JQuery\Input\buttonEx.js \
	$(ProjectDir)JQuery\Input\CheckBoxListEx.js \
	$(ProjectDir)JQuery\Input\DropDownListEx.js \
	$(ProjectDir)JQuery\Input\AutoComplete.js \
	$(ProjectDir)UI\WebControls\AppliedFilters.js

#In release mode we minify all files. The existing script file is always deleted
Release::
	del $(ProjectDir)\JQuery\Scripts\CommonScripts.min.js

Release:: $(SCRIPTFILES)
	!$(ProjectDir)Tools\jsmin.exe < $** >> $(ProjectDir)\JQuery\Scripts\CommonScripts.min.js

# In debug mode we simply concat the script files
Debug: $(ProjectDir)JQuery\Scripts\CommonScripts.min.js

$(ProjectDir)JQuery\Scripts\CommonScripts.min.js: $(SCRIPTFILES)
	del $@
	!type $** >> $@

	
