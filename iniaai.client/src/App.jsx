import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import './App.css';

const countries = ['Afghanistan', 'Albania', 'Algeria'];
const subjects = [
    'Gross domestic product, constant prices',
    'Volume of Imports of goods',
    'Total investment',
    'Inflation, average consumer prices'
];

const apiUrl = import.meta.env.VITE_API_URL;

function App() {
    console.log(apiUrl)
    const [selectedCountry, setSelectedCountry] = useState(countries[0]);
    const [selectedSubject, setSelectedSubject] = useState(subjects[0]);
    const [dataPoints, setDataPoints] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        fetchData();
    }, [selectedCountry, selectedSubject]);

    const fetchData = async () => {
        setLoading(true);
        try {
            const response = await axios.get(`${apiUrl}/Data`, {
                params: { country: selectedCountry, subject: selectedSubject }
            });
            const data = Array.isArray(response.data) ? response.data : [];
            setDataPoints(data);
            setError(null);
        } catch (error) {
            console.error('Error fetching data:', error);
            setDataPoints([]);
            setError('Failed to fetch data');
        } finally {
            setLoading(false);
        }
    };

    const chartOptions = {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            style: {
                fontFamily: "'Inter', sans-serif"
            }
        },
        title: {
            text: `${selectedSubject} in ${selectedCountry}`,
            style: { fontSize: '20px', fontWeight: '500' }
        },
        xAxis: {
            categories: dataPoints.map(point => point.year?.toString() ?? ''),
            title: { text: 'Year' },
            gridLineWidth: 1,
            gridLineColor: 'rgba(128, 128, 128, 0.1)'
        },
        yAxis: {
            title: { text: 'Value' },
            gridLineColor: 'rgba(128, 128, 128, 0.1)'
        },
        series: [{
            name: selectedSubject,
            data: dataPoints.map(point => point.value ?? 0),
            color: '#646cff',
            lineWidth: 3,
            marker: {
                radius: 6
            }
        }],
        tooltip: {
            borderRadius: 8,
            backgroundColor: 'rgba(255, 255, 255, 0.9)',
            borderWidth: 0,
            shadow: true
        }
    };

    return (
        <div className="container">
            <h1>Economic Data Visualization</h1>

            <div className="controls">
                <div className="select-wrapper">
                    <label htmlFor="country">Country</label>
                    <select
                        id="country"
                        value={selectedCountry}
                        onChange={(e) => setSelectedCountry(e.target.value)}
                    >
                        {countries.map((country) => (
                            <option key={country} value={country}>{country}</option>
                        ))}
                    </select>
                </div>

                <div className="select-wrapper">
                    <label htmlFor="subject">Subject Descriptor</label>
                    <select
                        id="subject"
                        value={selectedSubject}
                        onChange={(e) => setSelectedSubject(e.target.value)}
                    >
                        {subjects.map((subject) => (
                            <option key={subject} value={subject}>{subject}</option>
                        ))}
                    </select>
                </div>
            </div>

            {loading && <div className="loader">Loading...</div>}
            {error && <div className="error">{error}</div>}

            <div className="chart-container">
                <HighchartsReact highcharts={Highcharts} options={chartOptions} />
            </div>
        </div>
    );
}

export default App;