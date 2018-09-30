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
            format: 'json',
            metadata: {}
        }, {
            name: 'QA',
            description: 'QA test environment',
            configuration: 'hello editor',
            format: 'json',
            metadata: {}
        }, {
            name: 'Production',
            description: 'Production environment',
            configuration: 'hello editor',
            format: 'json',
            metadata: {}
        }]
    }
    async createEnvironment(env) {
        console.info(`Created new environment ${env.name}`)
    }

    async saveEnvironment(env) {
        console.info(`Saved ${env.name}`)
    }

    async deleteEnvironment(env) {
        console.info(`KABOOM! Deleted ${env}`)
    }
}