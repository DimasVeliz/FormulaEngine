import './App.css';
import React, { Component } from 'react'
import { Line } from 'react-chartjs-2'


const datos = () => {
  let DATA_COUNT = 12;
  let labels = [];
  for (let i = -7; i < DATA_COUNT - 7; ++i) {
    labels.push(i.toString());
  }
  let datapoints = [0, 20, 20, 60, 60, 120, NaN, 180, 120, 125, 105, 110, 170];
  let dataObjt = {
    labels: labels,
    datasets: [{
      label: 'Cubic interpolation',
      data: datapoints,
      borderColor: 'rgba(0, 255, 0, 1)',
      fill: false,
      tension: 0.4
    }
    ]
  };

  return dataObjt;
}

const opciones = () => {
  return {
    linetension: 0,
    responsive: true,
    plugins: {
      title: {
        display: true,
        text: 'Chart.js Line Chart - Cubic interpolation mode'
      },
    },
    interaction: {
      intersect: false,
    },
    scales: {
      x: {
        display: true,
        title: {
          display: true
        }
      },
      y: {
        display: true,
        title: {
          display: true,
          text: 'Value'
        },
        suggestedMin: -10,
        suggestedMax: 200
      }
    }
  }
}
class App extends Component {


  render() {


    return (
      <div className="App">
        <div>
          <textarea rows="20" cols="80" placeholder="write your code here"></textarea>

        </div>
        <button>Execute</button>
        <div className="grafico">
          <Line
          className="grafico"
            data={datos()}
            options={opciones()}
           
          />
        </div>

       


      </div>
    );
  }
}

export default App;
