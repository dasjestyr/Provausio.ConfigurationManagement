import axios from 'axios'
import config from '../configurations'

const fakeData = [{
    id: 1,
    name: 'DAS.Services.Notifications',
    description: 'Handles email and SMS messaging'
}, {
    id: 2,
    name: 'DAS.Services.Surveys.Command',
    description: 'Command host for surveys'
},{
    id: 3,
    name: 'DAS.Services.Surveys.Query',
    description: 'Query host for surveys'
},{
    id: 4,
    name: 'DAS.Services.Reviews.Mgr.Command',
    description: 'Command host for review management (review response)'
},{
    id: 5,
    name: 'DAS.Data.ETL.Reviews',
    description: 'Review ingestion extraction orchestrator.'
}]

export default class ApplicationService {

    constructor() {        
        this.client = axios.create(`http://${config.apiUrl}/applications`)            
    }

    async getApplications() {
        let response = await this.client.get('http://localhost:5000/applications')
        if(response.status == 200)
            return response.data.map(app => {
                return {
                    id: app.applicationId,
                    name: app.name,
                    description: app.description,
                    metadata: app.metadata
                }
            })

        console.error(response.status)
    }
}