import React, { useState } from 'react'
import Login from './Login'
import Create from './Create'

export default function Auth() {
  const [isLogin, setIsLogin] = useState(true)
  return (
    <div>
      {
        isLogin ?
          <Login changeScene={() => setIsLogin(false)} />
          :
          <Create changeScene={() => setIsLogin(true)} />
      }
    </div>
  )
}