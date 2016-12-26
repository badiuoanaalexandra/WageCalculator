import React from "react";
import moment from "moment";

export default React.createClass({
   render: function() {
        return (
         <div>
           <div>{moment(this.props.workingShift.StartTime).format('HH:mm')} -> {moment(this.props.workingShift.EndTime).format('HH:mm')}</div>
          </div>
        );
    }
});
