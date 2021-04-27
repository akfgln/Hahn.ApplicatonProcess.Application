# Hahn.ApplicatonProcess.Application

<p align="center">
  <img src="./wwwroot/apple-touch-icon.png" alt="ASP.NET Core & Aurelia 5" title="ASP.NET Core & Aurelia 5">
  <h1 align="center">ASP.NET Core 5 & Aurelia for Hahn<h1>
</p>

# Features:
<ul>
        <li><a href="https://get.asp.net/">ASP.NET Core</a> and <a href="https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx">C#</a> for cross-platform server-side code</li>
        <li><a href="http://aurelia.io/">Aurelia</a> and <a href="http://www.typescriptlang.org/">TypeScript</a> for client-side code</li>
        <li><a href="https://webpack.github.io/">Webpack</a> for building and bundling client-side resources</li>
        <li><a href="http://getbootstrap.com/">Bootstrap</a> for layout and styling</li>
</ul>

- **Testing**
  - Unit testing 

# Using

**Make sure you have at least Node 6.x or higher (w/ npm 3+) installed!**

Make sure you have .NET Core 5.0 installed and/or VS2019.
VS2019 will automatically install all the neccessary npm & .NET dependencies when you open the project.
If will not install try "npm install" and push F5 to start debugging!
SwaggerUI Address : http://localhost/swagger/index.html
AureliaUI Address : http://localhost

**Docker Build**
For docker usage in solution folder run "**docker build -t hahnproject:1.0 .**"
and run "**docker run -d -p 80:80 --name hahn hahnproject:1.0**"
