# AspNetCore.ClassicBundles - Home
This repository host the source code for AspNetCore.ClassicBundles

## Introduction

If you are new to ASP.Net core, you should know that JS & CSS bundles are no more handled by ASP.NET. Instead, you have to use somes post build tasks to create your bundles. Theses NuGet packages will help you
to restore the old fashioned bundles.

## What you need to know

AspNetCore.ClassicBundles basically works the same way as MVC 5.x bundles, but there is some differences. 
- Unlike System.Web.Optimization, it creates physically files. When you are declaring "~/bundles/my-little-bundle.js", it will create the file on your disk.
- Some functions are not yet implemented, or do not look like the original. You may only use .Include("~/js/my-bundle-without-any-wildcard.js"). Keep an eye on my TODO List to see what features will be backported.

## Getting started

You should look at the demo project, but here is the first steps to get it working.

### 

Install NuGet package(s) : 
- The first one is mandatory : AspNetCore.ClassicBundles
- The second one is optional : AspNetCore.ClassicBundle.TagHelpers. It will allow you to use some tag helpers instead of @Scripts.Render and @Styles.Render.

Note : Packages are currently available in a pre-release state.

Configure your BundleCollection on your Startup.cs

```c#
 // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
 public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
	...
	ConfigureBundles(BundleCollection.Instance, env);
	...
}

```

Then configure your bundles by adding the following method
```c#
public void ConfigureBundles(BundleCollection bundles, IHostingEnvironment env)
{
	bundles.IsMinMode = !env.IsDevelopment(); 
	bundles.RootPath = env.WebRootPath;
	//then, you can add your bundles like you used to do in MVC 5.x
	bundles.Add(new ScriptBundle("~/js/my-little-bundle.js")
                .Include("~/lib/jquery/dist/jquery.js")
                .Include("~/lib/bootstrap/dist/js/bootstrap.js")
                .Include("~/js/site.js"));

	bundles.Add(new StyleBundle("~/css/my-css-bundle.css")
                .Include("~/lib/bootstrap/dist/css/bootstrap.css"
                , "~/css/site.css"));
}
```

Now, you can use

```c#
@Styles.Render("~/css/my-css-bundle.css") 
```

and

```c#
 @Scripts.Render("~/js/my-little-bundle.js")
```

If you want to use  Asp.Net Core Tag Helpers, you can use

```html
 <vc:script-renderer bundle-path="~/js/my-little-bundle.js" />
```

And 

```html
 <vc:style-renderer bundle-path="~/css/my-css-bundle.css" /> 
```

## TODO
- [ ] More unit tests
- [ ] WildCard Patterns Support
- [ ] IncludeDirectory *Full* Support
- [ ] Performances improvements