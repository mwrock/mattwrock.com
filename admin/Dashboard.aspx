<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    <script src="jquery.masonry.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function(){
            $('#widgets').masonry({ singleMode: true, itemSelector: '.dashboardWidget' });
        });
    </script>
	<div class="content-box-outer">
		<div class="content-widgets-box-full">
            <div id="stats" style="width:20%;">
                <div class="dashboardStats">
                    <div class="rounded">
                        <h2>Stats</h2>
                        <ul>
                            <li>
                                <%=PostsPublished%> Post(s)<a class="viewAction endline" href="Posts/Posts.aspx">View all</a><br />
                                <%=DraftPostCount%> draft posts
                            </li>
                            <li>
                                <%=PagesCount%> Page(s)<a class="viewAction endline" href="Pages/Pages.aspx">View all</a><br />
                                <%=DraftPageCount%> draft pages
                            </li>
                            <li>
                                <%=CommentsAll%> Comment(s) <a class="viewAction endline" href="Comments/Approved.aspx">View all</a><br />
                                <%=CommentsUnapproved%> unapproved <a class="viewAction endline" href="Comments/Pending.aspx" >View all</a><br />
                                <%=CommentsSpam%> spam <a class="viewAction endline" href="Comments/Spam.aspx">View all</a>
                            </li>
                            <li>
                                <%=CategoriesCount%> Categorie(s)
                                    <a class="viewAction endline" href="Posts/Categories.aspx">View all</a>
                            </li>
                            <li>
                                <%=TagsCount%> Tag(s) 
                                    <a class="viewAction endline" href="Posts/Tags.aspx">View all</a>
                            </li>
                            <li>
                                <%=UsersCount%> User(s) 
                                    <a class="viewAction endline" href="Users/Users.aspx">View all</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div id="widgets" style="width:80%;">
                <div class="dashboardWidget">
                    <div class="rounded">
                        <h2>Draft posts <a class="addNew" href="Posts/Add_entry.aspx">Write new post</a></h2>
                        <ul id="DraftPosts" runat="server"></ul>
                    </div>
                </div>
                <div class="dashboardWidget rounded">
                    <div class="rounded">
                        <h2>Draft pages <a class="addNew" href="Pages/EditPage.aspx">Add new page</a></h2>
                        <ul id="DraftPages" runat="server"></ul>
                    </div>
                </div>
                <div class="dashboardWidget rounded">
                    <div class="rounded">
                        <h2>Trash</h2>
                        <a class="viewAction" href="Trash.aspx">View All</a> &nbsp;&nbsp;
                        <a class="deleteAction" href="#" onclick="return ProcessTrash('Purge', 'All');">Empty trash</a>
                    </div>
                </div>
                <div class="dashboardWidget rounded">
                    <div class="rounded">
                        <%=GetCommentsList()%>                      
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>

