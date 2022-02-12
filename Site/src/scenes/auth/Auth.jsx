import React, { useState } from 'react'
import { SignInScreen } from './SignIn'
import { SignUpScreen } from './SignUp'

export function Auth() {
  const [isLogin, setIsLogin] = useState(true)
  return (
    <div>
      {
        isLogin ?
          <SignInScreen changeScene={() => setIsLogin(false)} />
          :
          <SignUpScreen changeScene={() => setIsLogin(true)} />
      }
    </div>
  )
}