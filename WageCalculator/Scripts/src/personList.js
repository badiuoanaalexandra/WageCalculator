import React from "react";
import moment from "moment";

export default React.createClass({

render: function() {
    getMonthsArray:function() {
        
    },
    return (
        <div className="person-list">
            <div className="table-titles">
                <div>Name</div>
                <div>Total wage / March 2014</div>
            </div>
        {persons}
        </div>
    );
}
});

