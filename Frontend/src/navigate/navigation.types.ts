export interface IRoute {
  path: string
  element: JSX.Element
  routes?: IRoute[] // вложенные маршруты
}

export interface ILink {
  path: string
  name: string
}
