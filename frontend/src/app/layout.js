import { Inter } from 'next/font/google'
import './globals.css'
import '../../public/template/css/sb-admin-2.min.css'
import '../../public/template/css/fontawesome-free/css/all.min.css'
import Link from 'next/link'

const inter = Inter({ subsets: ['latin'] })

export const metadata = {
  title: 'App',
  description: 'App',
}

export default function RootLayout({ children }) {
  

  return (
    <html lang="pt-br">
      <body className={inter.className}>
           
        {children}
    
        <script src="/template/js/bootstrap.bundle.min.js"></script>
        <script src="/template/js/sb-admin-2.min.js"></script>
      </body>
    </html>
  )
}
