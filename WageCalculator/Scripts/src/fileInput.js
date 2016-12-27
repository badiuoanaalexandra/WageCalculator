import React from "react";

export default React.createClass({
render: function () {
    return (
      <div className={this.props.className}>
             <input accept=".csv" type="file" className="inputfile inputfile-1" id={this.props.id} ref="input" onChange={this.props.onChange} />
             <label htmlFor={this.props.id} ref="label">
					<img ref="img" src="/Content/Images/inputfile.svg" /><br />
					<span ref="text">Upload .csv file</span>
			 </label>
      </div>  
);
}
});

