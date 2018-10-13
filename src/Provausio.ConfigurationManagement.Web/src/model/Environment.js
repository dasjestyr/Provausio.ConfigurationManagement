import xid from 'xid-js'

export default class Environment {
    constructor(id) {
        this.id = id
    }

    static create() {
        const id = xid.next()
        const env = new Environment(id)
        env.name = ''
        env.description = ''
        env.configuration = {
            format: 'json',
            content: '{\n\t"foo": "bar"\n}',
            metadata: { isNew: true }
        }
        env.metadata = { }
        return env
    }
}