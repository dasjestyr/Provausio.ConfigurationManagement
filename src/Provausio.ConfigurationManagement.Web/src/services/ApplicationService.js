import axios from 'axios'
import config from '../configurations'

export default class ApplicationService {

    constructor() {        
        this.client = axios.create({
            baseURL: `${config.apiUrl}/applications`,
            headers: { 'Content-Type': 'application/json'}
        })            
    }

    async getApplications() {
        let response = await this.client.get('/')
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

    async createApplication(appInfo) {        
        let response = await this.client.post('/', {
            name: appInfo.name,
            description: appInfo.description
        })
        return response.data.applicationId
    }

    async getApplication(id) {
        let response = await this.client.get(`/${id}`)
        if(response.status == 200) {
            return {
                id: response.data.applicationId,
                name: response.data.name,
                description: response.data.description,
                metadata: response.data.metadata
            }
        }
    }

    async getEnvironments(appId) {
        return [{
            name: "Development",
            description: "Development/Integration environment configuration",
            configuration: '{\n\t"property" : "foo"\n}',
            format: 'json'
        }, {
            name: 'QA',
            description: 'QA test environment',
            configuration: 'hello editor',
            format: 'json'
        }, {
            name: 'Production',
            description: 'Production environment',
            configuration: 'hello editor',
            format: 'json'
        }]
    }

    async saveEnvironment(env) {
        console.info(`Bleep bloop, saved ${env.name}`)
    }
}