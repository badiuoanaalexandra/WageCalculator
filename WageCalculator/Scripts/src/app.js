import 'babel-polyfill';
 
import React from "react";
import ReactDOM from "react-dom";
import Main from "./main";

ReactDOM.render(
       <Main data={null} />,
        document.getElementById('main-wrapper')
            );
