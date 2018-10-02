import axios from 'axios'
import config from '../configurations'
import xid from 'xid-js'

export default class ApplicationService {

    constructor() {        
        this.client = axios.create({
            baseURL: `${config.apiUrl}/applications`,
            headers: { 'Content-Type': 'application/json'},
            validateStatus: function(status) {
                // will still get a client error in log anyway. This will at least make sure there aren't 2 errors
                return status < 1000 
            }
        })            
    }

    async getApplications() {
        let response = await this.client.get('/')
        if(response.status == 200){
            let applications = {}
            response.data.forEach(app => {
                applications[app.applicationId] = {
                    id: app.applicationId,
                    name: app.name,
                    description: app.description,
                    metadata: app.metadata
                }
            })
            return applications
        }

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
                metadata: response.data.metadata,
                environments: []
            }
        }
    }

    async deleteApplication(id) {
        await this.client.delete(`/${id}`)        
    }

    async getEnvironments(appId) {
        let response = await this.client.get(`/${appId}/environments`)
        let environments = []
        if(response.status == 200){
            console.log(response.data)
            response.data.forEach(env => {
                environments.push({
                    id: env.environmentId,
                    name: env.name,
                    description: env.description,
                    configuration: {
                        content: env.configuration.content,
                        format: env.configuration.format,
                        metadata: env.configuration.metadata
                    },
                    metadata: env.metadata
                })
            })
        } 
        return environments
    }
    async createEnvironment(appId, env) {
        delete env.metadata.isNew
        let response = await this.client.post(`/${appId}/environments`, {
            id: env.id,
            name: env.name,
            description: env.description,
            configuration: {
                content: env.configuration.content,
                format: env.configuration.format,
                metadata: env.configuration.metadata
            },
            metadata: env.metadata
        })
        
        if(response.status == 201) {
            console.log(`The new environment id is ${response.data.id}`)
            return response.data.id
        } else return null
    }

    async saveEnvironment(env) {
        console.info(`Saved ${env.name}`)
    }

    async deleteEnvironment(appId, environmentName) {
        console.info(`KABOOM! Deleted ${environmentName}`)
    }
}